function badgeClass(status) {
  if (status === 0) return 'badge badge-cors';
  if (status >= 200 && status < 300) return 'badge badge-ok';
  if (status === 401) return 'badge badge-401';
  if (status === 403) return 'badge badge-403';
  if (status === 429) return 'badge badge-429';
  return 'badge badge-error';
}

export default function StatusBadge({ status }) {
  return <span className={badgeClass(status)}>{status === 0 ? 'CORS' : status}</span>;
}
