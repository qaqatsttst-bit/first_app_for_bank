# 07_prometheus_metrics_dynamics.md

## 1. Назначение документа

Документ фиксирует правила работы с Prometheus, canonical metric queries, applicability matrix и range behavior V1.

Если документ конфликтует с `01_project_foundation.md`, источником истины остаётся foundation.

---

## 2. Роль Prometheus в V1

Prometheus используется как:
- источник временных рядов;
- источник range data по метрикам;
- источник динамики сервиса в карточке.

Prometheus **не** является источником истины по `current_status`.

---

## 3. Scope V1

### 3.1. MVP V1

В MVP поддерживаются диапазоны:
- `24h`
- `7d`

### 3.2. Full V1

Диапазон `30d` поддерживается только в Full V1 и только если retention в Prometheus позволяет достоверно вернуть такие данные.

---

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

---

## 5. UI semantics

### 5.1. Not applicable
Метрика по смыслу не применяется к сервису данного типа.

### 5.2. No data / Unavailable
Метрика должна быть, но данные сейчас отсутствуют или integration failed.

---

## 6. Canonical query inputs

Все Prometheus queries строятся через:
- `metrics_source_key`
- выбранный range
- выбранный step

Общий placeholder:
- `$metrics_source_key`
- `$range`
- `$step`

---

## 7. Canonical PromQL templates

> Примечание: точные metric names могут быть адаптированы под окружение, но canonical logical templates V1 фиксируются именно здесь.

### 7.1. Availability

```promql
avg_over_time(
  service_availability_ratio{metrics_source_key="$metrics_source_key"}[$range]
)
