# 06_status_model.md

## 1. Назначение документа

Документ формализует status model, precedence и transition rules V1.

Если документ конфликтует с `01_project_foundation.md`, foundation имеет приоритет.

## 2. Прикладные статусы

- `OK`
- `Degraded`
- `Down`
- `Unknown`
- `Maintenance`

## 3. External-to-internal mapping

Допустимые external status values:
- `UP` -> `OK`
- `DEGRADED` -> `Degraded`
- `DOWN` -> `Down`
- `UNKNOWN` -> `Unknown`

`Maintenance` не приходит из status source.

## 4. Источник истины

Для V1:
- основной источник `current_status` — авторитетный status source;
- UI не нормализует статус самостоятельно;
- Prometheus не является источником истины для `current_status`.

## 5. Precedence rules

### 5.1. Maintenance precedence
Если service находится в `Maintenance`, текущий прикладной статус отображается как `Maintenance` до ручного завершения maintenance.

### 5.2. External signal under maintenance
Если внешний source сообщает degradation while maintenance active:
- current status remains `Maintenance`;
- внешний проблемный сигнал показывается как secondary diagnostic signal.

## 6. Unknown rules

`Unknown` применяется если:
- status source недоступен и stale threshold exceeded;
- payload invalid и запись не может считаться доверенно обновлённой;
- external status cannot be interpreted;
- актуальный trusted status отсутствует.

## 7. Manual status rule

Allowed manual status only:
- `Maintenance`

`OK`, `Degraded`, `Down`, `Unknown` обычными пользователями вручную не устанавливаются.

## 8. Transition notes

Концептуально допустимы:
- `OK -> Degraded`
- `OK -> Down`
- `OK -> Unknown`
- `Degraded -> OK`
- `Degraded -> Down`
- `Degraded -> Unknown`
- `Down -> OK`
- `Down -> Degraded`
- `Down -> Unknown`
- `Unknown -> OK`
- `Unknown -> Degraded`
- `Unknown -> Down`
- `Any -> Maintenance`
- `Maintenance -> recalculated status based on current valid external state`

## 9. Transition recording

Любое значимое изменение статуса:
- создаёт `status_history` record;
- сохраняет source / actor context;
- может сопровождаться timeline event, если это manual or operationally important action.

## 10. Status freshness model

- `last_successful_status_refresh_at` хранится отдельно от статуса;
- пока stale threshold не превышен, допускается показ last known status;
- после превышения stale threshold сервис переходит в `Unknown`.

Exact numeric thresholds фиксируются в `10_non_functional_requirements.md`.
