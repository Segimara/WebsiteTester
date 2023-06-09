import type { TestResult } from "./TestResult";

export interface Link {
  id: string;
  url: string | null;
  createdOn: Date;
  testResults: TestResult[] | null;
}
