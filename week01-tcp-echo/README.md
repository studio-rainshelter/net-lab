# Week 01 — TCP Echo

## Concept

A TCP echo server/client pair is the simplest possible networking
exercise, but it exercises the fundamentals every later week builds on:
accepting connections with `TcpListener`, reading/writing bytes over a
`NetworkStream`, and structuring that I/O so it never blocks a thread.
Each accepted connection is handled on its own concurrent `Task`, so the
server can serve many clients at once without spinning up OS threads
per connection. Shutdown is cooperative: a single `CancellationToken`,
cancelled on Ctrl+C, threads through every awaited call so the process
exits cleanly instead of being killed mid-I/O.

## Layout

- `Server/` — accepts TCP connections on `127.0.0.1:5000` and echoes
  back whatever bytes each client sends.
- `Client/` — connects to the server, sends each line you type, and
  prints the echoed response.

## How to run

In one terminal:

```bash
cd week01-tcp-echo/Server
dotnet run
```

In another terminal:

```bash
cd week01-tcp-echo/Client
dotnet run
```

Type a line in the client terminal and press Enter — you should see it
echoed back. Ctrl+C stops either process cleanly.

> Verified: both `Server` and `Client` build cleanly with the .NET 10
> SDK (`dotnet build`, 0 warnings/0 errors) and a live round-trip
> (`dotnet run` server + piped client input) echoed back correctly.

## Notes for next time (fill this in!)

- **What I learned:**
- **What was hard:**
- **Benchmark results** (e.g. throughput/latency for N concurrent
  clients, buffer size tuning, etc.):
