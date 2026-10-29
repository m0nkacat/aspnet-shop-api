import { useEffect, useState } from 'react';
import StatusBadge from './StatusBadge.jsx';

export default function Reservations({ call, token, isAdmin, refreshKey, onChanged }) {
  const [items, setItems] = useState([]);
  const [total, setTotal] = useState(0);
  const [message, setMessage] = useState(null);

  useEffect(() => {
    call('/api/reservations?page=1&pageSize=20', { token }).then((response) => {
      if (response.ok) {
        setItems(response.data);
        setTotal(response.totalCount);
      }
    });
  }, [call, token, refreshKey]);

  const cancel = async (id) => {
    const response = await call(`/api/reservations/${id}/cancel`, { method: 'POST', token });
    setMessage(
      response.ok
        ? { status: response.status, text: `Резерв №${id} отменён, товар вернулся на склад.` }
        : { status: response.status, text: response.error },
    );
    if (response.ok) onChanged();
  };

  return (
    <section className="card">
      <h2>{isAdmin ? 'Все резервы' : 'Мои резервы'}</h2>
      <p className="muted">
        {isAdmin
          ? 'Администратор видит и может отменить любой резерв.'
          : 'Пользователь видит и может отменить только свои резервы.'}{' '}
        Всего: {total}.
      </p>
      {items.length === 0 ? (
        <p className="muted">Резервов пока нет.</p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>№</th>
              <th>Товар</th>
              <th>Кол-во</th>
              {isAdmin && <th>Владелец</th>}
              <th>Статус</th>
              <th />
            </tr>
          </thead>
          <tbody>
            {items.map((item) => (
              <tr key={item.reservationId}>
                <td>{item.reservationId}</td>
                <td>{item.product.productName}</td>
                <td>{item.quantity}</td>
                {isAdmin && <td>{item.user.username}</td>}
                <td>{item.isActive ? <span className="pill pill-active">активен</span> : <span className="pill">отменён</span>}</td>
                <td>{item.isActive && <button onClick={() => cancel(item.reservationId)}>Отменить</button>}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
      {message && (
        <div className={message.status < 300 ? 'result result-ok' : 'result result-error'}>
          <StatusBadge status={message.status} /> {message.text}
        </div>
      )}
    </section>
  );
}
