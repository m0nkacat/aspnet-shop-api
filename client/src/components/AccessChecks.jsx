import { useState } from 'react';
import StatusBadge from './StatusBadge.jsx';

const ALLOWED_ORIGIN = 'http://localhost:5173';

export default function AccessChecks({ call, token, isAdmin }) {
  const [results, setResults] = useState({});
  const [reservationId, setReservationId] = useState('1');

  const run = async (key, action) => {
    setResults((previous) => ({ ...previous, [key]: { pending: true } }));
    const outcome = await action();
    setResults((previous) => ({ ...previous, [key]: outcome }));
  };

  const single = (response) => ({
    statuses: [response.status],
    text: response.ok ? 'Доступ разрешён.' : response.error,
  });

  const checks = [
    {
      key: 'noToken',
      title: 'Запрос без токена',
      description: 'GET /api/reservations без заголовка Authorization.',
      expected: '401',
      action: async () => single(await call('/api/reservations')),
    },
    {
      key: 'badToken',
      title: 'Подделанный токен',
      description: 'Тот же запрос, но в токене изменена подпись.',
      expected: '401',
      action: async () => single(await call('/api/reservations', { token: token.slice(0, -4) + 'AAAA' })),
    },
    {
      key: 'adminOnly',
      title: 'Эндпоинт только для Admin',
      description: 'GET /api/users — политика AdminOnly (проверка по роли).',
      expected: isAdmin ? '200 (вы Admin)' : '403 (вы User)',
      action: async () => single(await call('/api/users?page=1&pageSize=3', { token })),
    },
    {
      key: 'owner',
      title: 'Резерв по номеру',
      description: 'GET /api/reservations/{id} — политика OwnerOrAdmin (проверка владельца).',
      expected: isAdmin ? '200 для любого резерва' : '200 для своего, 403 для чужого',
      input: true,
      action: async () => single(await call(`/api/reservations/${Number(reservationId) || 0}`, { token })),
    },
    {
      key: 'rateLimit',
      title: 'Лимит попыток входа',
      description: '6 входов подряд с неверным паролем. Лимит — 5 попыток в минуту, включая удачные. После проверки вход заблокирован до конца минуты.',
      expected: '401, затем 429',
      action: async () => {
        const statuses = [];
        let last;
        for (let i = 0; i < 6; i += 1) {
          last = await call('/api/auth/login', {
            method: 'POST',
            body: { username: 'rate-limit-check', password: 'wrong-password' },
          });
          statuses.push(last.status);
        }
        return { statuses, text: last.error };
      },
    },
    {
      key: 'cors',
      title: 'CORS',
      description: `API разрешает только ${ALLOWED_ORIGIN}. Эта страница открыта с ${window.location.origin}.`,
      expected: window.location.origin === ALLOWED_ORIGIN ? '200 (origin разрешён)' : 'CORS (origin запрещён)',
      action: async () => {
        const response = await call('/api/products?page=1&pageSize=1');
        return {
          statuses: [response.status],
          text: response.status === 0 ? response.error : `Ответ получен: origin ${window.location.origin} разрешён.`,
        };
      },
    },
  ];

  return (
    <section className="card">
      <h2>Проверка доступа</h2>
      <p className="muted">
        Каждая кнопка отправляет запрос, который должен быть отклонён или разрешён. Чтобы увидеть блокировку CORS,
        запустите клиент на другом порту: <code>npm run dev:blocked</code> → http://localhost:5174.
      </p>
      <div className="checks">
        {checks.map((check) => {
          const result = results[check.key];
          return (
            <div className="check" key={check.key}>
              <div className="check-head">
                <strong>{check.title}</strong>
                <span className="muted">ожидается: {check.expected}</span>
              </div>
              <div className="muted small">{check.description}</div>
              <div className="row">
                {check.input && (
                  <input
                    className="qty"
                    type="number"
                    min="1"
                    value={reservationId}
                    onChange={(e) => setReservationId(e.target.value)}
                  />
                )}
                <button onClick={() => run(check.key, check.action)}>Проверить</button>
              </div>
              {result?.pending && <div className="muted small">Отправка…</div>}
              {result && !result.pending && (
                <div className="check-result">
                  {result.statuses.map((status, index) => (
                    <StatusBadge key={index} status={status} />
                  ))}{' '}
                  <span>{result.text}</span>
                </div>
              )}
            </div>
          );
        })}
      </div>
    </section>
  );
}
