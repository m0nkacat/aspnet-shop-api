import { useEffect, useState } from 'react';
import StatusBadge from './StatusBadge.jsx';

export default function Products({ call, token, refreshKey, onChanged }) {
  const [products, setProducts] = useState([]);
  const [quantities, setQuantities] = useState({});
  const [message, setMessage] = useState(null);

  useEffect(() => {
    call('/api/products?page=1&pageSize=8').then((response) => {
      if (response.ok) setProducts(response.data);
    });
  }, [call, refreshKey]);

  const reserve = async (productId) => {
    const quantity = Number(quantities[productId] ?? 1);
    const response = await call('/api/reservations', { method: 'POST', token, body: { productId, quantity } });
    setMessage(
      response.ok
        ? { status: response.status, text: `Резерв №${response.data.reservationId} создан, остаток: ${response.data.product.stock}.` }
        : { status: response.status, text: response.error },
    );
    if (response.ok) onChanged();
  };

  return (
    <section className="card">
      <h2>Товары</h2>
      <p className="muted">Список доступен без входа. Резерв создаётся от имени пользователя из токена.</p>
      <table>
        <thead>
          <tr>
            <th>Товар</th>
            <th>Цена</th>
            <th>Остаток</th>
            <th />
          </tr>
        </thead>
        <tbody>
          {products.map((product) => (
            <tr key={product.productId}>
              <td>{product.productName}</td>
              <td className="nowrap">{product.price} ₽</td>
              <td>{product.stock}</td>
              <td className="actions">
                <div className="row">
                  <input
                    className="qty"
                    type="number"
                    min="1"
                    value={quantities[product.productId] ?? 1}
                    onChange={(e) => setQuantities({ ...quantities, [product.productId]: e.target.value })}
                  />
                  <button onClick={() => reserve(product.productId)}>Зарезервировать</button>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {message && (
        <div className={message.status < 300 ? 'result result-ok' : 'result result-error'}>
          <StatusBadge status={message.status} /> {message.text}
        </div>
      )}
    </section>
  );
}
