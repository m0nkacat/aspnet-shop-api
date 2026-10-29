import { useState } from 'react';
import { decodeToken } from '../api.js';
import AccessChecks from './AccessChecks.jsx';
import Products from './Products.jsx';
import Reservations from './Reservations.jsx';

export default function Dashboard({ auth, call, onLogout }) {
  const [refreshKey, setRefreshKey] = useState(0);
  const [showToken, setShowToken] = useState(false);
  const { user, accessToken, expiresAt } = auth;
  const isAdmin = user.role === 'Admin';
  const refresh = () => setRefreshKey((value) => value + 1);

  return (
    <>
      <section className="card user-card">
        <div>
          <div className="user-name">
            {user.username} <span className={isAdmin ? 'role role-admin' : 'role'}>{user.role}</span>
          </div>
          <div className="muted">
            Id {user.userId} · токен действует до {new Date(expiresAt).toLocaleTimeString('ru-RU')}
          </div>
        </div>
        <div className="row">
          <button onClick={() => setShowToken((value) => !value)}>{showToken ? 'Скрыть токен' : 'Показать токен'}</button>
          <button onClick={onLogout}>Выйти</button>
        </div>
        {showToken && (
          <div className="token">
            <div className="muted">Данные из JWT (claims):</div>
            <pre>{JSON.stringify(decodeToken(accessToken), null, 2)}</pre>
          </div>
        )}
      </section>

      <div className="grid">
        <Products call={call} token={accessToken} refreshKey={refreshKey} onChanged={refresh} />
        <Reservations call={call} token={accessToken} isAdmin={isAdmin} refreshKey={refreshKey} onChanged={refresh} />
      </div>

      <AccessChecks call={call} token={accessToken} isAdmin={isAdmin} />
    </>
  );
}
