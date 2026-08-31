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
2. `week02-async-socket` — async socket programming, multi-client handling
3. `week03-udp` — UDP datagrams
4. `week04-protocol-framing` — length-prefixed / delimiter-based framing
5. `week05-06-pipelines` — `System.IO.Pipelines` for high-throughput parsing
6. `week07-tls` — TLS via `SslStream` / mTLS
7. `week08-tcp-deep-dive` — Nagle/`NoDelay`, keep-alive, backlog,
   `SO_REUSEADDR`, graceful vs abrupt close, IOCP/epoll concurrency models
8. `week09-10-kcp` — reliable UDP via KCP (`kcp2k` / `KcpSharp`)
9. `week11-serialization` — Protobuf vs FlatBuffers vs MessagePack
10. `week12-14-web-protocols` — Kestrel, WebSocket, HTTP/2-3, gRPC, SignalR
11. `week15-16-magiconion` — MagicOnion (Unary + StreamingHub)
12. `week17-20-capstone` — integrated real-time project (design → build →
    harden → deploy)
13. `week21+` — advanced topics (memory pooling, QUIC, load balancing, etc.)

> Full week-by-week detail lives in `dotnet10-networking-roadmap.md` at
> the repo root — treat this outline as a quick index, that file as the
> source of truth.

---

## Learning mode: "Detailed explanation + self-implementation" (default)

This is the default and only mode for all future work in this repo, and
in any other project this file is copied into. No exceptions, no
conditions on prior state.

### Never do this by default
- Never write a complete, working solution end-to-end.
- Never fill in the core logic (socket handling, parsing, framing, etc.)
  on the user's behalf.
- Only provide a full implementation when the user explicitly says
  something like "초안 짜줘" / "정답 코드 보여줘" / "직접 구현해줘".

### Always do this — theory
- Explain the week's concept down to *why it works this way*
  (e.g. "why TCP guarantees ordering", "why Nagle's algorithm causes
  latency").
- Call out real-world pitfalls and failure modes, not just the happy path.
- Reference the exact type/method names and, where useful, official docs.

### Always do this — implementation guidance
- Name the exact classes/methods to use (e.g.
  `Socket.AcceptAsync(CancellationToken)`,
  `PipeReader.ReadAsync()`,
  `SequenceReader<byte>.TryReadLittleEndian`), including signatures,
  return types, and edge cases (timeouts, disconnects, partial reads).
- Break the implementation into ordered steps (e.g. "1) create listener
  → 2) accept loop → 3) per-client task → 4) propagate cancellation").
- Provide a **TODO skeleton only** — never the filled-in logic:
  ```csharp
  // TODO: Create a TcpListener and Start() it on the given port.
  // Hint: TcpListener(IPAddress, int) constructor.

  // TODO: In a loop, accept clients with AcceptTcpClientAsync(CancellationToken).
  //       Handle each client on its own Task (don't block the accept loop).
  ```
  Every TODO comment should say *what* to do and briefly *why*.

### Code review requests ("리뷰해줘")
- Point out problems and explain why they're problems; suggest a
  direction, but don't rewrite the code for the user.
- Flag serious bugs (resource leaks, deadlock risk) clearly, without
  silently fixing them.

### Explicit override
- Full implementations are only provided when explicitly requested, and
  even then every block should carry a short comment on *why* it's
  written that way.

### Weekly deliverable shape under this mode
```
/weekNN-topic/
  README.md    ← theory + step-by-step implementation guide + references
  Program.cs   ← TODO skeleton (to be filled in by the user)
  NOTES.md     ← written by the user: what was learned, blockers, questions
```

---

### 답변은 반드시 한국어로 할 것.
