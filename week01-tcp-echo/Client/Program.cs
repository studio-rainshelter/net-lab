using System.Net.Sockets;

// TODO: 서버와 동일하게 Console.CancelKeyPress -> cts.Cancel()을 배선하세요.
using var cts = new CancellationTokenSource();

// TODO: TcpClient를 생성하고 await client.ConnectAsync(host, port, cts.Token)로
//       서버에 연결하세요.

// TODO: 서버 응답을 계속 읽어 콘솔에 출력하는 백그라운드 Task를 하나 시작하세요
//       (예: var receiveTask = ReceiveLoopAsync(stream, cts.Token);).
//       왜 별도 Task인가: 표준 입력을 기다리는 동안에도 서버가 보낸 데이터를
//       즉시 받아 출력해야 하기 때문입니다 — 하나의 await 루프로는 둘을
//       동시에 할 수 없습니다.

// TODO: 메인 루프에서 Console.In.ReadLineAsync(cts.Token)으로 한 줄씩 입력받아,
//       UTF8로 인코딩한 뒤 개행을 붙여 stream.WriteAsync(...)로 전송하세요.
//       힌트: 동기 Console.ReadLine()은 취소 토큰을 받지 않으므로 이 규칙에서
//       금지된 블로킹 호출입니다.

static async Task ReceiveLoopAsync(NetworkStream stream, CancellationToken token)
{
    // TODO: 서버 처리 루프와 대칭적으로 구현하세요:
    //   버퍼를 하나 재사용하며 ReadAsync -> n == 0이면 서버가 연결을 닫은 것이므로
    //   종료 -> 받은 만큼(Span<byte> 또는 Memory<byte> 슬라이스)을
    //   Encoding.UTF8.GetString(...)로 디코딩해 콘솔에 출력.
}
