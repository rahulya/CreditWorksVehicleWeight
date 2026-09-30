# Category Configuration Automation Test Cases

## Overview
This document outlines all automation test cases for the `CategoryConfigurationService` which manages vehicle weight category classifications and validation.

---

## Test Suite: CategoryConfigurationTests

### Default Category Configuration
The test suite uses the following default category configuration:
- **Light**: 0 - 500 kg (inclusive minimum, exclusive maximum) - 🚵
- **Medium**: 500 - 2500 kg (inclusive minimum, exclusive maximum) - 🚙
- **Heavy**: 2500+ kg (inclusive minimum, unbounded maximum) - 🚚

---

## Test Cases

### 1. Category_uses_inclusive_minimum_and_exclusive_maximum
**Type:** Theory (Parameterized)  
**Purpose:** Validates that vehicle weight categories use inclusive minimum and exclusive maximum boundaries.

#### Test Data:
| Weight (kg) | Expected Category |
|-------------|------------------|
| 0.01        | Light            |
| 499.99      | Light            |
| 500.00      | Medium           |
| 2499.99     | Medium           |
| 2500.00     | Heavy            |
| 90000       | Heavy            |

**Preconditions:**
- Three vehicle categories exist with defined weight ranges

**Steps:**
1. Create vehicle categories with minimum and maximum weight bounds
2. Invoke `FindCategory()` with test weight values
3. Verify returned category name matches expected category

**Expected Result:** Each weight value is correctly classified into the appropriate category based on inclusive minimum and exclusive maximum boundaries.

**Assertions:**
```csharp
Assert.Equal(expected, CategoryConfigurationService.FindCategory(...))
```

---

### 2. Valid_contiguous_configuration_is_accepted_independent_of_input_order
**Type:** Fact  
**Purpose:** Validates that valid, contiguous category configurations are accepted regardless of input order.

**Preconditions:**
- Default category configuration exists

**Steps:**
1. Reverse the order of default categories
2. Invoke `Validate()` on the reversed list
3. Check that no validation errors are returned

**Expected Result:** Validation passes with an empty error list, confirming that order doesn't affect validation of valid configurations.

**Assertions:**
```csharp
Assert.Empty(CategoryConfigurationService.Validate(reversed))
```

---

### 3. Gap_is_rejected
**Type:** Fact  
**Purpose:** Validates that gaps in weight range coverage are detected and rejected.

**Preconditions:**
- Default category configuration exists

**Steps:**
1. Modify the first category (Light) to have a maximum weight of 499 kg (creating a gap between 499-500)
2. Invoke `Validate()` on the modified configuration
3. Check for validation error containing "gap or overlap"

**Expected Result:** Validation fails with an error message containing "gap or overlap".

**Assertions:**
```csharp
Assert.Contains(CategoryConfigurationService.Validate(ranges), 
	x => x.Contains("gap or overlap"))
```

---

### 4. Overlap_is_rejected
**Type:** Fact  
**Purpose:** Validates that overlapping weight ranges are detected and rejected.

**Preconditions:**
- Default category configuration exists

**Steps:**
1. Modify the first category (Light) to have a maximum weight of 501 kg (creating overlap with Medium starting at 500)
2. Invoke `Validate()` on the modified configuration
3. Check for validation error containing "gap or overlap"

**Expected Result:** Validation fails with an error message containing "gap or overlap".

**Assertions:**
```csharp
Assert.Contains(CategoryConfigurationService.Validate(ranges), 
	x => x.Contains("gap or overlap"))
```

---

### 5. Missing_unbounded_final_category_is_rejected
**Type:** Fact  
**Purpose:** Validates that the final category must be unbounded (no maximum weight limit).

**Preconditions:**
- Default category configuration exists

**Steps:**
1. Modify the final category (Heavy) to have a bounded maximum weight of 5000 kg instead of unbounded
2. Invoke `Validate()` on the modified configuration
3. Check for validation error containing "final category"

**Expected Result:** Validation fails with an error message containing "final category".

**Assertions:**
```csharp
Assert.Contains(CategoryConfigurationService.Validate(ranges), 
	x => x.Contains("final category"))
```

---

### 6. Reconfigured_ranges_reclassify_existing_weight_without_stored_category
**Type:** Fact  
**Purpose:** Validates that existing vehicle weights are reclassified correctly when category ranges are reconfigured.

**Preconditions:**
- An existing vehicle with weight of 2200 kg needs to be reclassified

**Test Data:**
- Existing vehicle weight: 2200 kg
- Original category configuration:
  - Light: 0 - 500 kg
  - Medium: 500 - 2500 kg (contains 2200 kg)
  - Heavy: 2500+ kg

- New category configuration:
  - Light: 0 - 500 kg
  - Medium: 500 - 2000 kg (no longer contains 2200 kg)
  - Heavy: 2000+ kg (now contains 2200 kg)

**Steps:**
1. Create a reconfigured category list with changed weight bounds
2. Invoke `FindCategory()` with the existing weight of 2200 kg
3. Verify returned category is "Heavy"

**Expected Result:** The weight 2200 kg is correctly reclassified to "Heavy" in the new configuration.

**Assertions:**
```csharp
Assert.Equal("Heavy", CategoryConfigurationService.FindCategory(changed, 2200)?.Name)
```

---

### 7. Empty_ranges_are_rejected
**Type:** Fact  
**Purpose:** Validates that an empty category configuration is rejected.

**Preconditions:**
- An empty category input list

**Steps:**
1. Create an empty list of categories
2. Invoke `Validate()` on the empty list
3. Check for validation error containing "At least one category is required."

**Expected Result:** Validation fails with an error message indicating that at least one category is required.

**Assertions:**
```csharp
Assert.Contains(CategoryConfigurationService.Validate(new List<CategoryInput>()), 
	x => x.Contains("At least one category is required."))
```

---

## Test Coverage Summary

| Category | Test Case | Status |
|----------|-----------|--------|
| Classification | Category_uses_inclusive_minimum_and_exclusive_maximum | ✓ |
| Configuration Validation | Valid_contiguous_configuration_is_accepted_independent_of_input_order | ✓ |
| Configuration Validation | Gap_is_rejected | ✓ |
| Configuration Validation | Overlap_is_rejected | ✓ |
| Configuration Validation | Missing_unbounded_final_category_is_rejected | ✓ |
| Reclassification | Reconfigured_ranges_reclassify_existing_weight_without_stored_category | ✓ |
| Configuration Validation | Empty_ranges_are_rejected | ✓ |

**Total Test Cases:** 12 (6 parameterized + 6 fact-based)  
**Total: 12 automated tests** ✓ All Passing

---

## Related Files

- **Source:** `CreditWorksVehicleWeight/Services/CategoryConfigurationService.cs`
- **Models:** 
  - `CreditWorksVehicleWeight/Models/CategoryInput.cs`
  - `DataAccess/Models/VehicleCategory.cs`
- **Test File:** `VehicleWeightTest/CategoryConfigurationTests.cs`

---

## Execution Notes

- **Framework:** xUnit.net
- **.NET Version:** .NET 10
- **Test Namespaces:**
  - `CreditWorksVehicleWeight.Services`
  - `CreditWorksVehicleWeight.Models`
  - `DataAccess.Models`

---

## Last Updated
Generated for: CategoryConfigurationTests Automation Test Suite
