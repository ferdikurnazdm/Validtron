(() => {
  const toggle = document.querySelector('[data-nav-toggle]');
  const menu = document.querySelector('[data-nav-menu]');
  if (toggle && menu) {
    toggle.addEventListener('click', () => {
      const open = menu.classList.toggle('open');
      toggle.setAttribute('aria-expanded', String(open));
    });
    menu.querySelectorAll('a').forEach(link => link.addEventListener('click', () => {
      menu.classList.remove('open');
      toggle.setAttribute('aria-expanded', 'false');
    }));
  }

  const reveal = document.querySelectorAll('.reveal');
  if ('IntersectionObserver' in window) {
    const observer = new IntersectionObserver(entries => {
      entries.forEach(entry => {
        if (entry.isIntersecting) {
          entry.target.classList.add('in-view');
          observer.unobserve(entry.target);
        }
      });
    }, { threshold: 0.12 });
    reveal.forEach(el => observer.observe(el));
  } else {
    reveal.forEach(el => el.classList.add('in-view'));
  }

  const steps = document.querySelectorAll('[data-code-tab]');
  const panels = document.querySelectorAll('[data-code-panel]');
  const fileLabel = document.querySelector('[data-code-file]');
  const files = { define: 'UserValidator.cs', validate: 'Validation.cs', inject: 'Program.cs' };
  steps.forEach(step => {
    step.addEventListener('click', () => {
      const target = step.dataset.codeTab;
      steps.forEach(item => item.classList.toggle('active', item === step));
      panels.forEach(panel => panel.classList.toggle('active', panel.dataset.codePanel === target));
      if (fileLabel) fileLabel.textContent = files[target] || 'Validtron.cs';
    });
  });

  const setCopied = button => {
    const original = button.textContent;
    button.textContent = 'Copied';
    setTimeout(() => { button.textContent = original; }, 1300);
  };

  document.querySelectorAll('[data-copy]').forEach(button => {
    button.addEventListener('click', async () => {
      try {
        await navigator.clipboard.writeText(button.dataset.copy);
        setCopied(button);
      } catch (_) {}
    });
  });

  document.querySelectorAll('[data-copy-target]').forEach(button => {
    button.addEventListener('click', async () => {
      let target;
      if (button.dataset.copyTarget === 'active-code') {
        target = document.querySelector('[data-code-panel].active code');
      } else {
        target = document.querySelector(`#${CSS.escape(button.dataset.copyTarget)} code`) || document.getElementById(button.dataset.copyTarget);
      }
      if (!target) return;
      try {
        await navigator.clipboard.writeText(target.textContent.trim());
        setCopied(button);
      } catch (_) {}
    });
  });

  const sections = document.querySelectorAll('.docs-section[id]');
  const sideLinks = document.querySelectorAll('.docs-sidebar a[href^="#"]');
  if (sections.length && sideLinks.length && 'IntersectionObserver' in window) {
    const sideObserver = new IntersectionObserver(entries => {
      entries.forEach(entry => {
        if (!entry.isIntersecting) return;
        sideLinks.forEach(link => link.classList.toggle('active', link.getAttribute('href') === `#${entry.target.id}`));
      });
    }, { rootMargin: '-20% 0px -68% 0px' });
    sections.forEach(section => sideObserver.observe(section));
  }
})();
