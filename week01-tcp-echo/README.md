# week01 — TCP 에코 서버/클라이언트

## 이론

### TCP는 왜 "스트림"인가
TCP는 메시지 경계를 보존하지 않는 **바이트 스트림** 프로토콜이다. 애플리케이션이
`Send()`를 세 번 호출해도 상대방은 `Receive()` 한 번에 세 번 분량을 몰아서 받을
수도, 한 번에 절반만 받을 수도 있다. 커널의 송신/수신 버퍼, Nagle 알고리즘,
네트워크 MTU가 개입해 애플리케이션이 보낸 write 단위와 상대방이 받는 read 단위가
전혀 다를 수 있기 때문이다. 그래서 "한 줄(line)"이나 "한 메시지" 단위로 통신하려면
애플리케이션 계층에서 직접 구분자(개행 등)나 길이 프리픽스로 프레이밍해야 한다.
이번 주 에코는 개행(`\n`) 기준으로 프레이밍한다 — 프레이밍 자체는 week04에서
본격적으로 다룬다.

### TCP가 순서와 신뢰성을 보장하는 이유
각 세그먼트에 시퀀스 번호를 붙이고, 수신 측이 ACK로 어디까지 받았는지 알려준다.
누락된 세그먼트는 타임아웃 또는 중복 ACK(빠른 재전송)로 재전송되고, 수신 측은
시퀀스 번호 순서대로 재조립한 뒤에야 애플리케이션에 전달한다. 이 재조립 버퍼
때문에 순서가 보장되지만, 앞 세그먼트가 도착하지 않으면 뒤 세그먼트가 이미
도착했어도 애플리케이션에 전달되지 않는 **HOL(Head-of-Line) 블로킹**이 발생한다.

### `AcceptAsync` / `ReadAsync`가 논블로킹인 이유
`Socket.AcceptTcpClientAsync(CancellationToken)`이나 `NetworkStream.ReadAsync`는
내부적으로 OS의 I/O 완료 통지(Linux epoll, Windows IOCP)를 기다리는 `Task`를
반환한다. 스레드를 블로킹하지 않고 반환하므로, 하나의 스레드가 수많은 연결을
동시에 대기(await)할 수 있다 — 이것이 C10K 문제를 해결하는 핵심 아이디어이며
week08에서 더 깊이 다룬다.

### 자주 겪는 함정
- **부분 수신(short read)**: `ReadAsync`가 요청한 만큼 채워서 돌려준다는 보장이
  없다. 반환값(읽은 바이트 수)이 0이면 상대방이 정상적으로 연결을 닫은 것
  (graceful close)이므로 루프를 종료해야 한다.
- **CancellationToken 미전파**: Ctrl+C를 눌러도 accept 루프나 read 루프가
  멈추지 않는다면 토큰이 실제로 그 호출까지 전달되지 않은 것이다.
- **클라이언트별 Task를 await하며 순차 처리**: Accept 루프 안에서 클라이언트
  처리를 `await`해버리면 두 번째 클라이언트는 첫 번째가 끝날 때까지 접속조차
  못 한다. 반드시 `_ = HandleClientAsync(...)`처럼 fire-and-forget(또는
  `Task` 목록에 모아 관리)해야 한다.
- **소켓 예외 흡수**: 클라이언트가 비정상 종료(RST)하면 `ReadAsync`가
  `SocketException`을 던진다. 이걸 잡지 않으면 서버 전체가 죽는다 — 반드시
  클라이언트별 처리 루프 안에서 try/catch.

## 구현 순서

1. **Server**: `TcpListener(IPAddress.Loopback, port)` 생성 → `Start()`
2. **Accept 루프**: `while (!token.IsCancellationRequested)` 안에서
   `AcceptTcpClientAsync(token)` 호출. 반환된 `TcpClient`마다
   `HandleClientAsync`를 **await 없이** 실행(fire-and-forget)해서 다음
   accept로 즉시 넘어가게 한다.
3. **클라이언트 처리**: `NetworkStream`에서 `ReadAsync(Memory<byte>, token)`로
   읽고, 읽은 만큼(`ReadOnlyMemory<byte>` 슬라이스)을 그대로
   `WriteAsync(ReadOnlyMemory<byte>, token)`로 되돌려 보낸다(에코).
   읽은 바이트 수가 0이면 루프를 빠져나와 연결을 정리한다.
4. **Client**: `TcpClient`로 연결 → 별도 Task에서 서버 응답을 계속 읽어
   콘솔에 출력, 메인 루프에서는 `Console.In.ReadLineAsync(token)`으로 표준
   입력을 읽어 서버로 전송.
5. **취소 토큰 배선**: `Console.CancelKeyPress` 핸들러에서
   `e.Cancel = true`로 프로세스 즉사를 막고, `CancellationTokenSource.Cancel()`을
   호출해 모든 await 지점에 취소를 전파한다.

## 참고 자료
- [`TcpListener.AcceptTcpClientAsync` 문서](https://learn.microsoft.com/dotnet/api/system.net.sockets.tcplistener.accepttcpclientasync)
- [`NetworkStream.ReadAsync` 문서](https://learn.microsoft.com/dotnet/api/system.net.sockets.networkstream.readasync)
- [`Console.CancelKeyPress` 이벤트](https://learn.microsoft.com/dotnet/api/system.console.cancelkeypress)

## 실행 방법 (직접 채워 넣은 후)
```bash
# 터미널 1
cd Server && dotnet run

# 터미널 2
cd Client && dotnet run
```

## NOTES.md
아직 없습니다 — TODO 스켈레톤을 채워 넣은 뒤, 이번 주 배운 것 / 막힌 부분 /
벤치마크(있다면)를 `NOTES.md`에 직접 정리해 주세요.
