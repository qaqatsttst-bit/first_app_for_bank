# 05_role_permission_matrix.md

## 1. Назначение документа

Документ фиксирует детальную permission-matrix V1.

---

## 2. Роли

- `Operator`
- `TechnicalSpecialist`
- `ServiceOwner`
- `Administrator`

---

## 3. Interpretation rules

### 3.1. ServiceOwner interpretation rule

Для owner-scoped permissions в V1 требуется одновременно:
- активная роль `ServiceOwner`;
- активная ownership relation к конкретному сервису.

Это правило применяется ко всем строкам матрицы, где указано `Yes (own service)`.

### 3.2. No scope without relation

Роль `ServiceOwner` без ownership relation к конкретному сервису не даёт права выполнять owner-scoped действия по этому сервису.

### 3.3. No owner rights from relation alone

Ownership relation без роли `ServiceOwner` не даёт owner-scoped прав.

### 3.4. Admin override

`Administrator` может выполнять административно разрешённые действия без ownership relation.

### 3.5. Enforcement rule

Все permissions должны применяться:
- в UI;
- в backend authorization;
- в application use cases.

UI-only hiding не считается enforcement.

---

## 4. Permission matrix

### 4.1. View permissions

| Action | Operator | TechnicalSpecialist | ServiceOwner | Administrator |
|---|---|---|---|---|
| View dashboard | Yes | Yes | Yes | Yes |
| View catalog | Yes | Yes | Yes | Yes |
| View service card | Yes | Yes | Yes | Yes |
| View status history | Yes | Yes | Yes | Yes |
| View timeline | Yes | Yes | Yes | Yes |
| View comments | Yes | Yes | Yes | Yes |
| View service links | Yes | Yes | Yes | Yes |
| View metrics | Yes | Yes | Yes | Yes |
| View role-safe diagnostics | Yes | Yes | Yes | Yes |
| View admin diagnostics | No | No | No | Yes |

### 4.2. Comment / incident-context actions

| Action | Operator | TechnicalSpecialist | ServiceOwner | Administrator |
|---|---|---|---|---|
| Add comment | No | Yes | Yes (own service) | Yes |
| Confirm problem | No | Yes | No | Yes |
| Revoke confirmation | No | Yes | No | Yes |

### 4.3. Link actions

| Action | Operator | TechnicalSpecialist | ServiceOwner | Administrator |
|---|---|---|---|---|
| Edit `runbook` | No | No | Yes (own service) | Yes |
| Edit `docs` | No | No | Yes (own service) | Yes |
| Edit `dashboard` | No | No | Yes (own service) | Yes |
| Edit `logs` | No | No | No | Yes |
| Edit `alerts` | No | No | No | Yes |
| Edit `other` | No | No | No | Yes |

### 4.4. Service governance actions

| Action | Operator | TechnicalSpecialist | ServiceOwner | Administrator |
|---|---|---|---|---|
| Create service | No | No | No | Yes |
| Activate/deactivate service | No | No | No | Yes |
| Change owner | No | No | No | Yes |
| Change category | No | No | No | Yes |
| Change criticality | No | No | No | Yes |
| Change service_type | No | No | No | Yes |
| Change integration keys | No | No | No | Yes |

### 4.5. User/role actions

| Action | Operator | TechnicalSpecialist | ServiceOwner | Administrator |
|---|---|---|---|---|
| Assign role | No | No | No | Yes |
| Remove role | No | No | No | Yes |
| Deactivate user | No | No | No | Yes |

### 4.6. Maintenance / admin actions

| Action | Operator | TechnicalSpecialist | ServiceOwner | Administrator |
|---|---|---|---|---|
| Start maintenance | No | No | No | Yes |
| End maintenance | No | No | No | Yes |
| Redact comment | No | No | No | Yes |
| Run backend-only admin flow | No | No | No | Yes / trusted ops only |

---

## 5. Consistency rule

Этот документ обязан быть синхронизирован с:
- `01_project_foundation.md`
- `04_glossary.md`

Если возникает конфликт, `01_project_foundation.md` имеет приоритет.
