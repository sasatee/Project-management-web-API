import { Card } from '@/components/ui/card';

export default function Users() {
  return (
    <div className="space-y-6">
      <h1 className="text-4xl font-bold">Users Management</h1>
      <Card className="p-6">
        <h2 className="text-2xl font-semibold mb-4">User List</h2>
        <p className="text-muted-foreground">
          Manage your users and their permissions here.
        </p>
      </Card>
    </div>
  );
}