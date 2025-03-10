import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { ThemeToggle } from "@/components/theme-toggle";

export default function Dashboard() {
  return (
    <div className="flex-1 space-y-4 p-4 md:p-8 pt-6">
      <div className="flex items-center justify-between">
        <h2 className="text-3xl font-bold tracking-tight">Dashboard</h2>
        <div className="flex items-center justify-end">
      <ThemeToggle />
      </div>
      </div>
      <div className="grid gap-4 md:grid-cols-3">
        <Card className="bg-card hover:bg-accent/10">
          <CardHeader className="flex flex-row items-center justify-between pb-2 space-y-0">
            <CardTitle className="text-sm font-medium">Analytics</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-xs text-muted-foreground">View your analytics here.</p>
          </CardContent>
        </Card>
        <Card className="bg-card hover:bg-accent/10">
          <CardHeader className="flex flex-row items-center justify-between pb-2 space-y-0">
            <CardTitle className="text-sm font-medium">Reports</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-xs text-muted-foreground">Access your reports here.</p>
          </CardContent>
        </Card>
        <Card className="bg-card hover:bg-accent/10">
          <CardHeader className="flex flex-row items-center justify-between pb-2 space-y-0">
            <CardTitle className="text-sm font-medium">Statistics</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-xs text-muted-foreground">Check your statistics here.</p>
          </CardContent>
        </Card>
      </div>
     
    </div>
  );
}