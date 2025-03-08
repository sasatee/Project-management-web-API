import { Card } from '@/components/ui/card';

export default function Dashboard() {
  return (
    <div className="space-y-6">
      <h1 className="text-4xl font-bold">Dashboard</h1>
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
        <Card className="p-6">
          <h2 className="text-2xl font-semibold">Analytics</h2>
          <p className="text-muted-foreground">View your analytics here.</p>
        </Card>
        <Card className="p-6">
          <h2 className="text-2xl font-semibold">Reports</h2>
          <p className="text-muted-foreground">Access your reports here.</p>
        </Card>
        <Card className="p-6">
          <h2 className="text-2xl font-semibold">Statistics</h2>
          <p className="text-muted-foreground">Check your statistics here.</p>
        </Card>
      </div>
    </div>
  );
}