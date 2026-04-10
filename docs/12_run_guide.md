# 12_run_guide.md

## 1. Назначение документа

Документ фиксирует operational-порядок backend-only administrative flows, verification steps и минимальные эксплуатационные требования для V1.

## 2. Допустимые backend-only administrative flows

Для V1 допустимы только:
- `controlled seed`
- `internal admin command`
- `migration-assisted assignment`
- эквивалентный backend-only controlled operational runbook

## 3. Кто имеет право запускать runbooks

Backend-only administrative runbooks могут запускать только:
- административная роль;
- ограниченный доверенный эксплуатационный персонал;
- сотрудники, явно включённые в operational access list окружения.

## 4. Preconditions for admin action

Перед запуском backend-only administrative action обязательно:
1. подтвердить основание для изменения;
2. проверить полномочия actor;
3. зафиксировать target entity;
4. зафиксировать intended change;
5. понять, какой audit trail должен быть создан;
6. понять, какой business effect ожидается после выполнения;
7. убедиться, что действие выполняется в доверенном серверном контуре.

## 5. Общий порядок выполнения backend-only admin action

1. Подтвердить основание для изменения.
2. Проверить полномочия actor.
3. Зафиксировать target entity и intended change.
4. Выполнить backend-only action через допустимый механизм.
5. Выполнить post-action verification.
6. Зафиксировать результат выполнения.
7. При ошибке или отсутствии audit trail — выполнить escalation/remediation.

## 6. Обязательный post-action verification

Проверить:
- business effect;
- наличие audit record;
- согласованность diagnostics / configuration state.
