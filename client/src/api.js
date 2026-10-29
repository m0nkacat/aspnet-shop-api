export const API_URL = import.meta.env.VITE_API_URL ?? 'http://localhost:5025';

const defaultMessages = {
  400: 'Некорректные данные.',
  401: 'Требуется вход в систему.',
  403: 'Недостаточно прав.',
  404: 'Не найдено.',
  409: 'Конфликт данных.',
  429: 'Слишком много запросов.',
  500: 'Ошибка сервера.',
};

function errorMessage(status, data) {
  if (data && typeof data === 'object') {
    if (data.error) return data.error;
    if (data.errors) return Object.values(data.errors).flat().join(' ');
  }
  return defaultMessages[status] ?? `Ошибка ${status}.`;
}

export async function request(path, { method = 'GET', body, token } = {}) {
  const headers = {};
  if (body !== undefined) headers['Content-Type'] = 'application/json';
  if (token) headers.Authorization = `Bearer ${token}`;

  let response;
  try {
    response = await fetch(API_URL + path, {
      method,
      headers,
      body: body !== undefined ? JSON.stringify(body) : undefined,
    });
  } catch {
    return {
      ok: false,
      status: 0,
      blocked: true,
      error: `Браузер не получил ответ: запрос с ${window.location.origin} заблокирован политикой CORS или API не запущен.`,
    };
  }

  const text = await response.text();
  let data = null;
  try {
    data = text ? JSON.parse(text) : null;
  } catch {
    data = text;
  }

  return {
    ok: response.ok,
    status: response.status,
    data,
    error: response.ok ? null : errorMessage(response.status, data),
    retryAfter: Number(response.headers.get('Retry-After')) || null,
    totalCount: Number(response.headers.get('X-Total-Count')) || 0,
  };
}

export function decodeToken(token) {
  try {
    const payload = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/');
    const json = decodeURIComponent(
      atob(payload)
        .split('')
        .map((c) => '%' + c.charCodeAt(0).toString(16).padStart(2, '0'))
        .join(''),
    );
    return JSON.parse(json);
  } catch {
    return null;
  }
}
