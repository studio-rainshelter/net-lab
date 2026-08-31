# net-lab

Weekly hands-on exercises in C# 14 / .NET 10, focused on networking:
sockets, pipelines, TCP/UDP, TLS, and web protocols — building up to a
finished, production-style project.

## Rules for every week's code

- Target `net10.0`; C# 14 language features are fair game (primary
  constructors, collection expressions, the `field` keyword, etc.).
- All I/O is async: `async`/`await` throughout, and every awaitable call
  takes a `CancellationToken` wired to app shutdown (e.g. Ctrl+C via
  `Console.CancelKeyPress`).
- Prefer `Span<T>`/`Memory<T>` (and their `ReadOnly` counterparts) over
  extra `byte[]` copies for buffer handling.
- No blocking calls: no `.Result`, `.Wait()`, `Thread.Sleep`, or
  synchronous stdin/socket reads inside async flows (use
  `Console.In.ReadLineAsync(cancellationToken)`, `stream.ReadAsync`, etc.).
- Each week is a self-contained console app (or a couple of small
  projects, e.g. a server and a client) under `/weekNN-topic`, runnable
  directly with `dotnet run`.
- Each week folder has its own `README.md` documenting: what was
  learned, what was hard, and any benchmark results.

## Structure

- `/weekNN-topic/README.md` — notes for that week
- `/weekNN-topic/<Project>/*.csproj` — one or more minimal console
  projects for that week's exercise

## Curriculum outline (adjust as we go)

1. `week01-tcp-echo` — TCP echo server/client (socket fundamentals)
2. `week02-udp` — UDP datagrams
3. `week03-pipelines` — `System.IO.Pipelines` for high-throughput parsing
4. `week04-tls` — TLS via `SslStream`
5. `week05+` — HTTP/web protocols, building toward a finished project
