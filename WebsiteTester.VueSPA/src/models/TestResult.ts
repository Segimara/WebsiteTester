export interface TestResult {
  createdOn: Date;
  url: string | null;
  renderTimeMilliseconds: number;
  isInSitemap: boolean;
  isInWebsite: boolean;
}