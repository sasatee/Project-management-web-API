import { Card } from '@/components/ui/card';

export default function Settings() {
  return (
    <div className="space-y-6">
      <h1 className="text-4xl font-bold">Settings</h1>
      <Card className="p-6">
        <h2 className="text-2xl font-semibold mb-4">System Settings</h2>
        <p className="text-muted-foreground">
          Configure your system settings here.
        </p>
      </Card>
    </div>
  );
}