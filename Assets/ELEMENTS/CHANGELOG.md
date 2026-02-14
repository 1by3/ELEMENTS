# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0-beta.2] - 2026-02-13

### Added
- Checkbox component
- Table components (Table, TableHead, TableHeader, TableBody, TableRow, TableCell, TableFooter, TableCaption)
- Extended styles USS stylesheet
- Typography styles with bundled Inter and Lexend fonts

### Changed
- Split default styles into default and extended stylesheets
- Updated MenuItem to work like a button
- Cleaned up problematic default styles

### Fixed
- Gap utility class

## [2.0.0-beta.1] - 2025-02-12

### Changed
- Replaced `RenderComponent<T>()` extension methods with `RenderElement()` on VisualElement, UIDocument, and EditorWindow
- `RenderElement()` accepts `IElement` or `Component` instances directly instead of using generics
- Added `additive` parameter to `RenderElement()` to allow appending without clearing existing content

### Removed
- Removed `ElementEditorWindow<T>` base class in favor of `EditorWindow.RenderElement()` extension method

### Added
- `EditorWindowExtensions` with `RenderElement()` extension methods for `EditorWindow`

## [2.0.0-beta.0] - 2025-01-01

### Changed
- Major refactor to ELEMENTS for V2
- Code-first, component-based UI framework for Unity UI Toolkit

### Added
- Component base class for building reactive UI
- ElementPortal for rendering components into UIDocument
- UI Elements: Alert, Button, ContextMenu, Dialog, DialogContent, Group, HorizontalGroup, Image, Label, Loader, MenuDivider, MenuItem, Popover, ProgressBar, ScrollView, TabItem, TabList, TextField, VerticalGroup
- Extension methods for VisualElement, UIDocument, and Popover
- ComponentFactory and ElementsUI helpers
- Conditional rendering with If helper
- Default USS styles
- Editor window base class (ElementEditorWindow)
