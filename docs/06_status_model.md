# 06_status_model.md

## 1. Назначение документа

Документ формализует status model, precedence и transition rules V1.

---

## 2. Прикладные статусы

- `OK`
- `Degraded`
- `Down`
- `Unknown`
- `Maintenance`

---

## 3. External-to-internal mapping

- `UP` -> `OK`
- `DEGRADED` -> `Degraded`
- `DOWN` -> `Down`
- `UNKNOWN` -> `Unknown`

---

## 4. Status precedence

### 4.1. Maintenance precedence

Если service находится в `Maintenance`, текущий прикладной статус отображается как `Maintenance` до ручного завершения maintenance.

### 4.2. External signal under maintenance

Если внешний source сообщает degradation while maintenance active:
- current status remains `Maintenance`
- external signal shown as secondary diagnostic signal

---

## 5. Unknown rules

`Unknown` применяется если:
- status source недоступен;
- status stale threshold exceeded;
- payload invalid;
- external status cannot be interpreted.

---

## 6. Transition recording

Любое значимое изменение статуса:
- creates `status_history` record;
- may create timeline event if manual/important;
- should preserve actor/source context.

---

## 7. Manual status rule

Allowed manual status only:
- `Maintenance`

`OK/Degraded/Down/Unknown` обычными пользователями вручную не устанавливаются.

---

## 8. Transition notes

Allowed conceptual transitions:
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
- Any -> `Maintenance`
- `Maintenance` -> recalculated status based on current valid external state

---

## 9. Consistency rule

Этот документ обязан быть синхронизирован с `01_project_foundation.md`.
