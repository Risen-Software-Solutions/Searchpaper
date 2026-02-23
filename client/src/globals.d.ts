interface ToastAlert {
  id: string;
  message: string;
  success: boolean;
}

type Nullable<T> = T | null;

interface User {
  id: string;
  email: string;
  fullName: string;
  role: string;
}
