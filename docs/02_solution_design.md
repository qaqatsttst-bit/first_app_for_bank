# 02_solution_design.md

## 1. Назначение документа

Документ фиксирует exact contracts, integration flows, validation/API shapes и solution-level решения, которые не должны храниться в `01_project_foundation.md`.

Если документ конфликтует с foundation, источником истины остаётся `01_project_foundation.md`.

---

## 2. Основные integration flows

### 2.1. Status source polling flow

1. Backend по расписанию обращается к status source bulk endpoint.
2. Response валидируется по contract-shape.
3. Для каждого item происходит mapping по `status_source_key`.
4. Внешний статус нормализуется во внутренний прикладной статус.
5. Если статус изменился — создаётся запись в `status_history`.
6. Обновляются `last_successful_status_refresh_at` и связанная diagnostics информация.
7. При ошибках интеграции обновляется `integration_sync_state`.

### 2.2. Metrics retrieval flow

1. UI или backend инициирует range query к Prometheus через backend.
2. Используются canonical templates из `07_prometheus_metrics_dynamics.md`.
3. Query строится через `metrics_source_key`.
4. Для `Not applicable` query не выполняется.
5. При metrics failure overview страницы не ломается.

---

## 3. Exact contract for status source

### 3.1. Endpoint

`GET /api/v1/statuses`

### 3.2. Query parameters

- `include_inactive=false`
- `cursor` — optional, если source использует pagination
- `limit` — optional

Для V1 preferred mode — один bulk response без необходимости paging.

### 3.3. Response shape

```json
{
  "version": "v1",
  "generated_at": "2026-04-10T10:15:00Z",
  "source_name": "central-status-service",
  "items": [
    {
      "status_source_key": "mega-payments-gateway",
      "external_status": "UP",
      "status_timestamp": "2026-04-10T10:14:32Z",
      "message": "Service is healthy"
    }
  ]
}
