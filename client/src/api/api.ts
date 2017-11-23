import 'isomorphic-fetch';

type PayloadType = 'file' | 'json';

interface Payload {
  type: PayloadType;
  data: any;
}

const call = <T>(url: string, method: 'GET' | 'POST' | 'PATCH' | 'DELETE', payload?: Payload) => {
  const body = payload
    ? payload.type === 'json' && payload.data ? JSON.stringify(payload.data) : payload.data
    : undefined;

  const headers =
    payload && payload.type === 'json' ? { 'Content-Type': 'application/json' } : undefined;

  const options = {
    method,
    body,
    credentials: 'include',
    headers
  };

  return fetch(url, options as any)
    .then((response: any) => {
      if (response.status === 204) {
        return Promise.resolve({});
      }
      const contentType = response.headers.get('content-type');
      return contentType && contentType.includes('application/json')
        ? response.json()
        : response.text();
    })
    .then((json: any) => {
      return json as T;
    });
};

export function get<T>(url: string) {
  return call<T>(url, 'GET');
}

export function post<T>(url: string, data: any, type: PayloadType = 'json') {
  return call<T>(url, 'POST', { type, data });
}
export function patch<T>(url: string, data: any, type: PayloadType = 'json') {
  return call<T>(url, 'PATCH', { type, data });
}

export function httpDelete(url: string) {
  return call(url, 'DELETE');
}
