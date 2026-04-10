# 10_non_functional_requirements.md

## 1. Назначение документа

Документ фиксирует расширенные нефункциональные требования V1 и является **единственным источником exact numeric thresholds**.

## 2. Performance baseline

Для V1 принимаются следующие baseline targets:
- до `500` active services;
- до `50` concurrent users;
- главная страница `p95 <= 2 секунды`;
- каталог `p95 <= 2 секунды`;
- карточка сервиса без metrics section `p95 <= 2 секунды`;
- manual refresh `<= 5 секунд`;
- timeout status source `5 секунд`;
- timeout Prometheus `5 секунд`;
- не более `1` retry в интерактивном пользовательском сценарии.

## 3. Operational thresholds

### 3.1. Status stale thresholds
- stale threshold для статуса: `5 минут`
- stale threshold для метрик: `10 минут`

### 3.2. Unhealthy integration threshold
Интеграция считается `unhealthy`, если:
- `3` подряд неуспешных polling cycles;
- или нет `last_successful_sync_at` дольше `15 минут`;
- или partial failures повторяются более `3` последовательных циклов.

## 4. Logging retention

### 4.1. Audit log
- minimum online retention: `365 дней`

### 4.2. Technical logs
- hot retention: `30 дней`

## 5. Session and re-auth policy

Рекомендуемый baseline:
- idle timeout: `30 минут`
- absolute session lifetime: `8 часов`

## 6. Security and reliability

- backend-only access к внешним интеграциям;
- конечные таймауты внешних интеграций;
- отсутствие бесконечных retries в интерактивных сценариях;
- metrics integration failure не должна ломать Overview / History / Comments / Timeline / Links.
