import { useCallback, useRef, useState } from 'react';
import { API_URL, request } from './api.js';
import AuthScreen from './components/AuthScreen.jsx';
import Dashboard from './components/Dashboard.jsx';
import RequestLog from './components/RequestLog.jsx';

const STORAGE_KEY = 'shop.auth';

function loadAuth() {
  try {
    const saved = JSON.parse(localStorage.getItem(STORAGE_KEY));
    if (saved && new Date(saved.expiresAt) > new Date()) return saved;
  } catch {
    return null;
  }
  return null;
}

export default function App() {
  const [auth, setAuth] = useState(loadAuth);
  const [log, setLog] = useState([]);
  const [notice, setNotice] = useState(null);
  const [corsBlocked, setCorsBlocked] = useState(false);
  const tokenRef = useRef(auth?.accessToken);

  const saveAuth = useCallback((value) => {
    tokenRef.current = value?.accessToken;
    setAuth(value);
    if (value) localStorage.setItem(STORAGE_KEY, JSON.stringify(value));
    else localStorage.removeItem(STORAGE_KEY);
  }, []);

  const call = useCallback(
    async (path, options = {}) => {
      const result = await request(path, options);

      setLog((previous) =>
        [
          {
            id: crypto.randomUUID(),
            time: new Date().toLocaleTimeString('ru-RU'),
            method: options.method ?? 'GET',
            path,
            withToken: Boolean(options.token),
            status: result.status,
            message: result.error ?? 'OK',
          },
          ...previous,
        ].slice(0, 40),
      );

      setCorsBlocked(result.status === 0);

      if (result.status === 401 && options.token && options.token === tokenRef.current) {
        saveAuth(null);
        setNotice(`Сессия завершена: ${result.error}`);
      }

      return result;
    },
    [saveAuth],
  );

  const handleLogin = (data) => {
    setNotice(null);
    saveAuth(data);
  };

  const handleLogout = () => {
    saveAuth(null);
    setNotice('Вы вышли из системы. Токен удалён из браузера.');
  };

  return (
    <div className="app">
      <header className="topbar">
        <div className="brand">
          Shop client <span className="muted">· JWT · CORS · rate limiting</span>
        </div>
        <div className="origins">
          <span>
            Фронтенд: <code>{window.location.origin}</code>
          </span>
          <span>
            API: <code>{API_URL}</code>
          </span>
        </div>
      </header>

      {corsBlocked && (
        <div className="banner banner-danger">
          <strong>Запрос заблокирован браузером.</strong> Origin <code>{window.location.origin}</code> не входит в
          список <code>Cors:AllowedOrigins</code> API (или API не запущен). Сервер мог ответить, но браузер не отдал
          ответ странице.
        </div>
      )}

      {notice && (
        <div className="banner banner-info">
          {notice}
          <button className="link" onClick={() => setNotice(null)}>
            Скрыть
          </button>
        </div>
      )}

      <main className="content">
        {auth ? (
          <Dashboard auth={auth} call={call} onLogout={handleLogout} />
        ) : (
          <AuthScreen call={call} onLogin={handleLogin} />
        )}
        <RequestLog entries={log} onClear={() => setLog([])} />
      </main>
    </div>
  );
}
