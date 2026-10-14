# Hexagonal nucleus: compatible first increment

## Objective
Prepare Application organization without changing existing behavior or teammate contracts. This is transitional groundwork, not completed hexagonal isolation.

## Authorized scope
Only feature/hexagonal-nucleo. Preserve namespaces, signatures, constructors and source bytes. Move eight Service interfaces to Application/Ports/Input, two repository interfaces to Application/Ports/Output, three existing DTOs to Application/DTOs, and seven CRUD Service implementations to Application/Services. Keep AuthService implementation in its existing location until security ports/adapters and DI are agreed.

## Constraints
No edits to Controllers, Program.cs, Models, DbContext, concrete repositories, project/configuration files, main, develop or teammate branches. Preserve untracked .codegraph/ and Lab05-Andrew-Alca/doc.md. No dotnet execution by the agent. No push or merge under this increment. No new features, DTO signatures, pure-domain rules or security ports without agreed contracts.

## Tasks
- [x] N1: Relocate the twenty files without changing bytes; verify original destinations, uniqueness, namespaces and untouched teammate surfaces. Route: delegated; trigger: multi-file preparation and move. Structural outcome verified; work-unit closure/commit remains pending N2.
- [x] N2: Build observed by explicit user-authorized agent execution (exit 0, 0 warnings/errors); independent structural check passed. Work-unit commit `3e88c837bf2ceb0c395ce1f6f7a00114c0d962fe`, twenty 100% renames, no source insertions/deletions. Native mode verified off (global); no native review started or approval claimed. Route: delegated build/checks and parent Git state/actions.

## Acceptance and checks
- Exactly twenty moves, no duplicates or source-content modifications.
- No teammate-owned file changes.
- Compilation must be reported by the user before claiming compile readiness.
- Functional regression remains pending; unchanged bytes alone are structural proof, not endpoint proof.
- Test-first exception: source relocation only, no meaningful behavioral RED; perform byte identity/structure checks and user build.

## Progress
N1 structural outcome verified: eight input ports, two output ports, three existing DTOs and seven CRUD Services relocated (20 files). SHA256 identity passed for all 20 targets; original files absent; exactly one definition per moved type. Namespaces, signatures and constructors are unchanged because file bytes are unchanged. All 21 protected files in Controllers, Models, Data, Repositories/Implements, Program.cs and AuthService.cs retained their SHA256 hashes. No prior tracked changes were present. Branch feature/hexagonal-nucleo and main/develop refs remained unchanged. The SDK-style Web project uses default source inclusion with no explicit exclusions. Build, tests and runtime/endpoint checks were not run: the user must execute dotnet commands. Branch point a8ae292. Commit pending; authored code lines forecast 0 excluding pure rename accounting. Delivery strategy ask-on-risk; no PR planned. Review/risk assessment pending until verified candidate; no review approval claimed.

Compatibility debt: the existing namespace layout and entity-based contracts remain transitional. Domain isolation, CRUD DTO decoupling and security ports/adapters require coordinated agreements with teammates. AuthService implementation intentionally remains in Services/Implements; only its interface moved. This increment does not establish completed hexagonal isolation.

## Independent verification

Build observed after explicit one-command user authorization: `dotnet build .\Lab05-Andrew-Alca\Lab05-Andrew-Alca.csproj`, exit 0, 0 warnings, 0 errors; restore up to date, target net10.0 DLL generated. Branch remained feature/hexagonal-nucleo; no extra tracked source modifications. This overrides manual-only execution for this invocation, not as a permanent preference change. N2 remains open for review/commit closure; endpoint regression, push and merge were not performed.

20/20 destination Git blobs match original HEAD paths through configured clean filters; 20 originals are absent and 21 protected files unchanged. Windows autocrlf explains raw HEAD/checkout differences; the writer compared pre/post SHA256. Compilation and endpoints remain pending. Native mode status unavailable (Git directory access denied); assessment high/unassessable (untracked inventory undeclared). No native approval or mode change; resolve checks before work-unit closure.

## Next step

Closure evidence: both tasks structurally complete and compilation observed. Native status now verified with required filesystem access: off, decided by global. Earlier permission/untracked assessment failures are historical, not approval. User explicitly authorized commit/push only to feature/hexagonal-nucleo. Endpoint regression and complete hexagonal decoupling remain outside this increment; no merge to develop or main. Publish the implementation and this progress record, then coordinate integration with teammates.
User runs `dotnet build .\Lab05-Andrew-Alca\Lab05-Andrew-Alca.csproj` from the repository root and reports the result before N2. Future DTO/security contract coordination is outside this first increment. Functional regression proof and commit evidence remain pending.

