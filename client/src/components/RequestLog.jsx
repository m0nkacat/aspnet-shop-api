import StatusBadge from './StatusBadge.jsx';

export default function RequestLog({ entries, onClear }) {
  return (
    <section className="card">
      <div className="check-head">
        <h2>Журнал запросов</h2>
        {entries.length > 0 && (
          <button className="link" onClick={onClear}>
            Очистить
          </button>
        )}
      </div>
      {entries.length === 0 ? (
        <p className="muted">Запросов пока не было.</p>
      ) : (
        <table className="log">
          <thead>
            <tr>
              <th>Время</th>
              <th>Запрос</th>
              <th>Токен</th>
              <th>Статус</th>
              <th>Ответ</th>
            </tr>
          </thead>
          <tbody>
            {entries.map((entry) => (
              <tr key={entry.id}>
                <td className="muted">{entry.time}</td>
                <td>
                  <code>
                    {entry.method} {entry.path}
                  </code>
                </td>
                <td>{entry.withToken ? 'да' : '—'}</td>
                <td>
                  <StatusBadge status={entry.status} />
                </td>
                <td className="small">{entry.message}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </section>
  );
}
