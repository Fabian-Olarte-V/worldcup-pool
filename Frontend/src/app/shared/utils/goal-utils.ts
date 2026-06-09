export const MIN_GOALS = 0;
export const MAX_GOALS = 20;

export function normalizeGoalValue(value: number | null): number | null {
  if (value === null || Number.isNaN(value)) {
    return null;
  }

  return Math.min(MAX_GOALS, Math.max(MIN_GOALS, value));
}

export function isGoalValid(value: number | null): value is number {
  return value !== null && Number.isInteger(value) && value >= MIN_GOALS && value <= MAX_GOALS;
}
