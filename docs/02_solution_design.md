# 02_solution_design.md

## 1. Назначение документа

Документ фиксирует solution-level дизайн V1:
- integration flows;
- external/internal contracts;
- validation/result shapes;
- background processing model;
- authentication flow;
- boundaries between layers.

Если документ конфликтует с `01_project_foundation.md`, foundation имеет приоритет.

## 2. Архитектурная схема

V1 использует следующие слои:
- `Domain`
- `Application`
- `Infrastructure`
- `Web`

### 2.1. Domain
Содержит предметные сущности, value objects, доменные правила и инварианты.

### 2.2. Application
Содержит use cases, orchestration, validation, authorization enforcement и structured results.

### 2.3. Infrastructure
Содержит PostgreSQL persistence, OIDC integration, status source integration, Prometheus access, audit/technical logging.

### 2.4. Web
Содержит Blazor Server UI, page composition, view models и role-aware presentation.

## 3. External integrations

### 3.1. Authoritative status source
High-level rules:
- backend-only access;
- server-to-server authentication;
- bulk retrieval обязателен;
- статусные данные должны быть сопоставимы с `status_source_key`.

### 3.2. Prometheus
Prometheus используется для:
- range metrics;
- metric dynamics;
- service card metrics section.

Prometheus не является источником истины для `current_status`.

### 3.3. OIDC
OIDC используется для:
- аутентификации пользователя;
- получения обязательных claims `sub` и `email`.

## 4. Background processing

### 4.1. Status refresh
Backend выполняет controlled polling авторитетного status source.

### 4.2. Metrics refresh
Metrics могут:
- загружаться on-demand;
- использовать caching / prefetch strategy;
- обновляться через controlled backend path.

## 5. Structured validation/result model

Для административных и activation-сценариев рекомендуется единая модель результата:

```json
{
  "is_success": false,
  "error_code": "service_activation_missing_owner",
  "message": "Activation failed",
  "details": [
    {
      "field": "owner",
      "code": "required"
    }
  ],
  "correlation_id": "..."
}
```

## 6. Authorization boundary

Authorization должен применяться в:
- UI;
- application use case;
- backend endpoint / handler boundary.

Owner-scoped access требует:
- активной роли `ServiceOwner`;
- активной ownership relation к target service.

## 7. Integration contract artifacts

Для production-grade реализации должны существовать отдельные contract artifacts для:
- status source payload;
- validation result catalog;
- audit event catalog;
- admin command/result shapes.
