import { PropsWithChildren } from "react";

export function Example({
  title,
  children,
}: PropsWithChildren<{ title?: string }>) {
  return (
    <div className="flex flex-col rounded-xl border p-1 bg-fd-card">
      {title && <span className="font-medium text-sm p-1">{title}</span>}
      <div className="rounded-lg border p-2 bg-[#fff]">{children}</div>
    </div>
  );
}
