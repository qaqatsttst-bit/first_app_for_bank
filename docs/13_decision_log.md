# 13_decision_log.md

## 1. Назначение документа

Документ фиксирует ключевые решения V1 в формате ADR-lite.

## 2. Decision log

### D-001. Authoritative status source is mandatory
`current_status` определяется из отдельного авторитетного status source, а не из Prometheus или UI.

### D-002. Prometheus is metrics-only for current V1 semantics
Prometheus используется для метрик и динамики, но не для определения `current_status`.

### D-003. Maintenance is the only manual status
Вручную устанавливается только `Maintenance`.

### D-004. Comments are append-only
Обычные роли не редактируют и не удаляют comments.

### D-005. ServiceOwner role != ownership relation
Owner-scoped actions требуют и роли `ServiceOwner`, и ownership relation.

### D-006. Backend-only admin flows are allowed but controlled
Административные изменения в MVP могут выполняться через controlled backend-only flows.

### D-007. Audit is separate from technical logs
Audit trail не смешивается логически с technical logs.
