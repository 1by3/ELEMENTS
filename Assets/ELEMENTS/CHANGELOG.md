# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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
