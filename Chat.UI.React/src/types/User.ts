export interface AuthResult {
    success: boolean;
    token: string;
    message: string;
}

export interface LoginInfo {
    username: string;
    password: string;
}