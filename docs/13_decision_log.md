# 13_decision_log.md

## 1. Назначение документа

Документ фиксирует принятые решения V1, чтобы они не переоткрывались неформально в чатах, коде или вспомогательных заметках.

---

## 2. Decision log

### D-001. V1 roles are fixed
В V1 существуют только:
- `Operator`
- `TechnicalSpecialist`
- `ServiceOwner`
- `Administrator`

Новые роли не создаются.

### D-002. Authoritative status source
`current_status` определяется из отдельного status source, а не из Prometheus.

### D-003. Only Maintenance is manual
Ручной прикладной статус V1 — только `Maintenance`.

### D-004. Service links are part of MVP
Базовые service links входят в MVP, а Full V1 добавляет richer presentation/admin helpers.

### D-005. Backend-only admin flows are allowed in MVP
До появления UI для Users/Roles используются:
- `controlled seed`
- `internal admin command`
- `migration-assisted assignment`
- backend-only controlled runbook

### D-006. Backend-only admin flows require audit
Все backend-only administrative changes обязаны оставлять audit trail.

### D-007. service_links model is mandatory
`service_links` — обязательная сущность V1.

### D-008. Link edit scope
`ServiceOwner` может редактировать только:
- `runbook`
- `docs`
- `dashboard`

`logs`, `alerts`, `other` — admin only.

### D-009. 30d metrics are Full V1 only
`30d` range допустим только во Full V1 и только при достаточном retention.

### D-010. Unhealthy integration threshold lives outside foundation
Точные thresholds фиксируются в `10_non_functional_requirements.md`.

### D-011. Exact contracts live in 02_solution_design.md
JSON/status-source contract и validation shapes не живут в foundation.

### D-012. Bootstrap first admin is one-time
Bootstrap-admin действует до появления первого active admin и затем должен быть отключён.

### D-013. V1 glossary is canonical
Канонический словарь терминов V1 фиксируется в `04_glossary.md`.

Новые термины и переопределение уже согласованных терминов не должны появляться в реализации и вспомогательных документах без явного обновления glossary и, при необходимости, foundation.

### D-014. Local setup documentation is separated from admin run guide
Локальный запуск, environment bootstrap, migrations и developer run instructions не живут в `12_run_guide.md`.

Эти темы выносятся в `14_environment_and_run.md`.

`12_run_guide.md` остаётся документом про backend-only administrative flows, verification и remediation.

### D-015. V1 does not introduce separate service notes entity
В V1 не вводится отдельная прикладная сущность `service_notes`.

Пользовательский operational context представляется через:
- `service_comments`
- `service_timeline_events`
- audit semantics, где это уместно

### D-016. Owner-scoped access requires both role and ownership relation
Owner-scoped permissions требуют одновременно:
- активную роль `ServiceOwner`;
- активную ownership relation к конкретному сервису.

Ни роль без relation, ни relation без роли не дают owner-scoped прав.

### D-017. Exact numeric thresholds live only in 10_non_functional_requirements.md
Все точные stale / retry / unhealthy numeric values должны канонически жить в `10_non_functional_requirements.md`.

Другие документы могут использовать их семантически, но не должны становиться вторым источником numeric truth.

### D-018. 09_pages_and_navigation.md is a page-behavior contract
`09_pages_and_navigation.md` фиксирует обязательные page/section states, markers и visibility boundaries.

Он не является pixel-perfect design spec, но является обязательным UI behavior contract document.

### D-019. Literal startup commands and config keys live near code
`14_environment_and_run.md` фиксирует process-level и setup-level правила запуска.

Literal startup commands, exact migration commands, env key names и executable startup artifacts должны жить рядом с кодом и быть непосредственным source of execution truth.

---

## 3. Change control

Изменение любого решения из этого журнала требует:
1. обновления `01_project_foundation.md`;
2. обновления соответствующих дочерних документов;
3. обновления реализации.

---

## 4. Consistency rule

Этот документ обязан быть синхронизирован с:
- `01_project_foundation.md`
- `04_glossary.md`
- `10_non_functional_requirements.md`
