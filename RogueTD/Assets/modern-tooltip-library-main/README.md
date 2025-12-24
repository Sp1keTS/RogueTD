# Modern Tooltip Library

A lightweight, customizable tooltip solution that works on **WordPress, Shopify, and custom projects**.

## ✨ Features
- Smooth fade-in/out (no flicker)
- Custom positions: `top`, `bottom`, `left`, `right`
- Themes: `success`, `warning`, `error`, `info`
- Auto-init with `data-tooltip` attributes
- Programmatic API (create, show, hide, destroy)
- Proper cleanup with `destroy()`

## 🚀 Usage
```html
<button data-tooltip="Hello world!">Hover me 1</button>

<button data-tooltip="Hello world!" data-tooltip-position="right">Hover me 2</button>

<button data-tooltip="Hello world!" data-tooltip-position="right" data-tooltip-theme="info">Hover me 3</button>

<script src="tooltip.js"></script>
<script>
  Tooltip.init();
</script>
```

## 📦 Programmatic Example
```js
Tooltip.create(myButton, {
  content: "Dynamic tooltip",
  position: "right",
  theme: "success"
});
```

## 🛠️ Commands
- `Tooltip.init()` → initialize all `[data-tooltip]`
- `Tooltip.create(el, options)` → bind tooltip to element
- `Tooltip.show(el)` / `Tooltip.hide(el)` → manually control
- `Tooltip.destroy(el)` → unbind + cleanup

---
© 2025 Modern Tooltip Library
