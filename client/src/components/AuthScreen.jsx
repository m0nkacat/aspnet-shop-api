import { useEffect, useState } from 'react';
import StatusBadge from './StatusBadge.jsx';

export default function AuthScreen({ call, onLogin }) {
  const [mode, setMode] = useState('login');
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [result, setResult] = useState(null);
  const [busy, setBusy] = useState(false);
  const [waitSeconds, setWaitSeconds] = useState(0);

  useEffect(() => {
    if (waitSeconds <= 0) return undefined;
    const timer = setTimeout(() => setWaitSeconds((value) => value - 1), 1000);
    return () => clearTimeout(timer);
  }, [waitSeconds]);

  const submit = async (event) => {
    event.preventDefault();
    if (!username.trim() || !password) {
      setResult({ status: 400, error: 'Введите логин и пароль.' });
      return;
    }

    setBusy(true);
    const path = mode === 'login' ? '/api/auth/login' : '/api/auth/register';
    const response = await call(path, { method: 'POST', body: { username: username.trim(), password } });
    setBusy(false);

    if (response.ok) {
      onLogin(response.data);
      return;
    }

    setResult(response);
    if (response.status === 429) setWaitSeconds(response.retryAfter ?? 60);
  };

  const switchMode = (value) => {
    setMode(value);
    setResult(null);
  };

  return (
    <section className="card auth-card">
      <div className="tabs">
        <button className={mode === 'login' ? 'tab active' : 'tab'} onClick={() => switchMode('login')}>
          Вход
        </button>
        <button className={mode === 'register' ? 'tab active' : 'tab'} onClick={() => switchMode('register')}>
          Регистрация
        </button>
      </div>

      <form onSubmit={submit} className="form">
        <label>
          Логин
          <input value={username} onChange={(e) => setUsername(e.target.value)} autoComplete="username" />
        </label>
        <label>
          Пароль
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            autoComplete={mode === 'login' ? 'current-password' : 'new-password'}
          />
        </label>
        <button className="primary" disabled={busy || waitSeconds > 0}>
          {waitSeconds > 0
            ? `Подождите ${waitSeconds} с`
            : mode === 'login'
              ? 'Войти'
              : 'Зарегистрироваться'}
        </button>
      </form>

      {mode === 'register' && <p className="hint">Новый пользователь получает роль User. Пароль — не короче 6 символов.</p>}
      {mode === 'login' && <p className="hint">Вход ограничен: не больше 5 попыток в минуту, дальше API отвечает 429.</p>}

      {result && (
        <div className="result result-error">
          <StatusBadge status={result.status} /> {result.error}
        </div>
      )}
    </section>
  );
}
