# 05_role_permission_matrix.md

## 1. Назначение документа

Документ фиксирует детальную permission-matrix V1.

Если документ конфликтует с `01_project_foundation.md`, foundation имеет приоритет.

## 2. Роли

- `Operator`
- `TechnicalSpecialist`
- `ServiceOwner`
- `Administrator`

## 3. Interpretation rules

### 3.1. Owner-scoped rule
Для owner-scoped permissions требуется одновременно:
- активная роль `ServiceOwner`;
- активная ownership relation к конкретному сервису.

### 3.2. No scope without relation
Роль `ServiceOwner` без ownership relation не даёт owner-scoped прав по конкретному сервису.

### 3.3. No owner rights from relation alone
Ownership relation без роли `ServiceOwner` не даёт owner-scoped прав.

### 3.4. Admin override
`Administrator` может выполнять административно разрешённые действия без ownership relation.

### 3.5. Enforcement rule
Все permissions должны применяться:
- в UI;
- в backend authorization;
- в application use cases.

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

### 4.2. Comments / incident-context actions

| Action | Operator | TechnicalSpecialist | ServiceOwner | Administrator |
|---|---|---|---|---|
| Add comment | No | Yes | Yes (own service) | Yes |
| Confirm problem | No | Yes | No | Yes |
| Revoke problem confirmation | No | Yes | No | Yes |

### 4.3. Service metadata actions

| Action | Operator | TechnicalSpecialist | ServiceOwner | Administrator |
|---|---|---|---|---|
| Edit service description | No | No | Yes (own service) | Yes |
| Edit service owners | No | No | No | Yes |
| Edit category | No | No | No | Yes |
| Edit criticality | No | No | No | Yes |
| Edit service type | No | No | No | Yes |
| Edit integration keys | No | No | No | Yes |
| Create service | No | No | No | Yes |
| Activate service | No | No | No | Yes |
| Deactivate service | No | No | No | Yes |

### 4.4. Link actions

| Action | Operator | TechnicalSpecialist | ServiceOwner | Administrator |
|---|---|---|---|---|
| Create `runbook` link | No | No | Yes (own service) | Yes |
| Create `docs` link | No | No | Yes (own service) | Yes |
| Create `dashboard` link | No | No | Yes (own service) | Yes |
| Create `logs` link | No | No | No | Yes |
| Create `alerts` link | No | No | No | Yes |
| Create `other` link | No | No | No | Yes |
| Edit `runbook` / `docs` / `dashboard` | No | No | Yes (own service) | Yes |
| Edit `logs` / `alerts` / `other` | No | No | No | Yes |
| Deactivate any link | No | No | Limited to own allowed types | Yes |

### 4.5. Maintenance / admin actions

| Action | Operator | TechnicalSpecialist | ServiceOwner | Administrator |
|---|---|---|---|---|
| Start maintenance | No | No | No | Yes |
| End maintenance | No | No | No | Yes |
| Redact comment | No | No | No | Yes |
| Assign role | No | No | No | Yes |
| Remove role | No | No | No | Yes |
| Deactivate user | No | No | No | Yes |
| Run backend-only admin flow | No | No | No | Yes / trusted ops only |
