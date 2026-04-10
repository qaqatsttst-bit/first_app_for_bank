# 07_prometheus_metrics_dynamics.md

## 1. Назначение документа

Документ фиксирует правила работы с Prometheus, metric applicability, ranges, canonical query policy и degraded semantics для metrics section V1.

## 2. Роль Prometheus в V1

Prometheus используется как:
- источник временных рядов;
- источник range data по метрикам;
- источник динамики сервиса в карточке.

Prometheus **не** является источником истины по `current_status`.

## 3. Scope V1

### 3.1. MVP V1
В MVP поддерживаются диапазоны:
- `24h`
- `7d`

### 3.2. Full V1
Диапазон `30d` поддерживается только в Full V1 и только если retention в Prometheus позволяет достоверно вернуть такие данные.

## 4. Metric applicability matrix

### 4.1. Required metrics
- `availability` — applicable для всех `service_type`
- `error_rate` — applicable для `Internal`, `External`, `Integration`
- `latency_p95` — applicable для `Internal`, `External`, `Integration`

### 4.2. Provider rules
Для `Provider`:
- `availability` — required
- `error_rate` — `Not applicable`
- `latency_p95` — `Not applicable`

## 5. UI semantics

- `Not applicable` — метрика по смыслу не применяется к сервису данного типа.
- `No data` — метрика должна быть, но данных сейчас нет.
- `Unavailable` — данные не могут быть получены из-за integration failure, timeout или query error.

## 6. Canonical query inputs

Все Prometheus queries строятся через:
- `metrics_source_key`
- выбранный range
- выбранный step

## 7. Canonical metric families

Для V1 фиксируются:
- `availability`
- `error_rate`
- `latency_p95`

## 8. Query policy

- Разные страницы не должны использовать ad hoc query logic для одной и той же метрики.
- Все queries должны строиться через `metrics_source_key`.
- Одна и та же метрика должна иметь единый canonical query template per metric family.

## 9. Degraded behavior

Если Prometheus недоступен:
- service overview остаётся доступным;
- history / comments / timeline / links остаются доступными;
- metrics section показывает degraded state;
- `current_status` не меняется только из-за падения Prometheus.
