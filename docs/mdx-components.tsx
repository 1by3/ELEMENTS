import defaultMdxComponents from "fumadocs-ui/mdx";
import type { MDXComponents } from "mdx/types";
import { Example } from "./components/Example";
import { Button } from "./components/ui/button";
import { Label } from "./components/ui/label";
import { cn } from "./lib/utils";

// use this function to get MDX components, you will need it for rendering MDX
export function getMDXComponents(components?: MDXComponents): MDXComponents {
  return {
    ...defaultMdxComponents,
    ...components,
    Example: Example,
    Label: ({ children, ...props }) => <div {...props}>{children}</div>,
    Button: Button,
    HorizontalGroup: ({ children, className, ...props }) => (
      <div className={cn("flex", className)} {...props}>
        {children}
      </div>
    ),
    VerticalGroup: ({ children, className, ...props }) => (
      <div className={cn("flex", "flex-col", className)} {...props}>
        {children}
      </div>
    ),
  };
}
