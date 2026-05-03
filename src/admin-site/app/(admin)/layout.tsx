import Sidebar from "@/features/shared/components/Sidebar";
import Navbar from "@/features/shared/components/Navbar";
import FloatingDrawerButton from "@/features/shared/components/FloatingDrawerButton";
import DrawerCheckbox from "@/features/shared/components/DrawerCheckbox";

export default function AdminLayout({ children }: { children: React.ReactNode }) {
  return (
    <div className="drawer md:drawer-open">
      {/* Toggle for open sidebar */}
      <DrawerCheckbox />

      {/* Content */}
      <div className="drawer-content flex flex-col relative">
        <Navbar />
        <FloatingDrawerButton />

        <main className="flex-1 p-6 bg-base-100">{children}</main>
      </div>

      {/* Sidebar */}
      <div className="drawer-side">
        <label htmlFor="admin-drawer" className="drawer-overlay"></label>

        <Sidebar />
      </div>
    </div>
  );
}
