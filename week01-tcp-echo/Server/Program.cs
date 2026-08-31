using System.Net;
using System.Net.Sockets;

// TODO: Ctrl+C(SIGINT)를 받아도 프로세스가 즉시 죽지 않도록
//       Console.CancelKeyPress 이벤트를 구독하고 e.Cancel = true로 막은 뒤,
//       아래 CancellationTokenSource를 Cancel()하세요.
//       왜: 기본 동작은 프로세스를 즉시 종료시켜서, 진행 중인 accept/read
//       루프가 정리될 기회를 얻지 못합니다.
using var cts = new CancellationTokenSource();

// TODO: TcpListener(IPAddress.Loopback, port)를 생성하고 Start()하세요.
//       포트는 상수로 두거나 args[0]에서 파싱하세요.
// var listener = new TcpListener(...);

Console.WriteLine("TODO: 서버가 준비되면 여기에 리스닝 주소를 출력하세요.");

// TODO: while (!cts.Token.IsCancellationRequested) 루프 안에서
//       listener.AcceptTcpClientAsync(cts.Token)로 클라이언트를 계속 받으세요.
//       힌트: 반환 타입은 ValueTask<TcpClient>이며, 토큰이 취소되면
//       OperationCanceledException을 던집니다 — 루프 밖에서 잡아서
//       정상 종료로 처리하세요.
//
//       중요: 여기서 클라이언트 처리를 await하면 안 됩니다. await하는 순간
//       두 번째 클라이언트는 첫 번째가 끝날 때까지 accept조차 되지 않습니다.
//       HandleClientAsync(client, cts.Token)를 fire-and-forget으로 실행하거나
//       (예: _ = HandleClientAsync(...)) 진행 중인 Task들을 List<Task>에
//       모아뒀다가 종료 시 Task.WhenAll로 정리하세요.

static async Task HandleClientAsync(TcpClient client, CancellationToken token)
{
    // TODO: using으로 client를 감싸 스코프를 벗어나면 자동으로 Dispose되게 하세요.
    // await using var stream = client.GetStream();

    // TODO: Memory<byte> 버퍼(예: new byte[1024])를 하나 할당해 루프 밖에서
    //       재사용하세요. 매 반복마다 새 byte[]를 만들면 불필요한 GC 압박이
    //       생깁니다 — 이번 주 규칙(Span<T>/Memory<T> 우선)의 이유이기도 합니다.

    // TODO: while (true) 루프:
    //   1) int n = await stream.ReadAsync(buffer, token);
    //   2) n == 0이면 상대방이 정상 종료(FIN)한 것이므로 break;
    //   3) 받은 만큼만(buffer.AsMemory(0, n)) 그대로
    //      await stream.WriteAsync(buffer.AsMemory(0, n), token)로 에코
    //
    //   왜 n == 0을 확인해야 하는가: TCP는 스트림이라 ReadAsync가 요청한
    //   만큼 채워준다는 보장이 없고, 0은 "더 이상 데이터 없음(연결 종료)"을
    //   뜻하는 유일한 신호입니다. 이걸 놓치면 무한 루프가 됩니다.

    // TODO: try/catch로 SocketException, OperationCanceledException을 잡아
    //       클라이언트 한 명의 비정상 종료가 서버 전체를 죽이지 않게 하세요.
}
