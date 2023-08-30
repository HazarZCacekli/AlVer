// Selecting the sidebarpp and buttons
const sidebarp = document.querySelector(".sidebarp");
const sidebarpOpenBtn = document.querySelector("#sidebarp-open");
const sidebarpCloseBtn = document.querySelector("#sidebarp-close");
const sidebarpLockBtn = document.querySelector("#lock-icon");

// Function to toggle the lock state of the sidebarp
const toggleLock = () => {
  sidebarp.classList.toggle("locked");
  // If the sidebarp is not locked
  if (!sidebarp.classList.contains("locked")) {
    sidebarp.classList.add("hoverable");
    sidebarpLockBtn.classList.replace("bx-lock-alt", "bx-lock-open-alt");
  } else {
    sidebarp.classList.remove("hoverable");
    sidebarpLockBtn.classList.replace("bx-lock-open-alt", "bx-lock-alt");
  }
};

// Function to hide the sidebarp when the mouse leaves
const hidesidebarp = () => {
  if (sidebarp.classList.contains("hoverable")) {
    sidebarp.classList.add("close");
  }
};

// Function to show the sidebarp when the mouse enter
const showsidebarp = () => {
  if (sidebarp.classList.contains("hoverable")) {
    sidebarp.classList.remove("close");
  }
};

// Function to show and hide the sidebarp
const togglesidebarp = () => {
  sidebarp.classList.toggle("close");
};

// If the window width is less than 800px, close the sidebarp and remove hoverability and lock
if (window.innerWidth < 800) {
  sidebarp.classList.add("close");
  sidebarp.classList.remove("locked");
  sidebarp.classList.remove("hoverable");
}

// Adding event listeners to buttons and sidebarp for the corresponding actions
sidebarpLockBtn.addEventListener("click", toggleLock);
sidebarp.addEventListener("mouseleave", hidesidebarp);
sidebarp.addEventListener("mouseenter", showsidebarp);
sidebarpOpenBtn.addEventListener("click", togglesidebarp);
sidebarpCloseBtn.addEventListener("click", togglesidebarp);
