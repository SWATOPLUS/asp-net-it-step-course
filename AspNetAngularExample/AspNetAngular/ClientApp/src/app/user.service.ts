import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";

@Injectable({providedIn: 'root'})
export class UserService {
    private jwt = '';

    constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    }

    public signIn(username: string, password: string) {
        this.httpClient.post<any>(this.baseUrl + 'api/user/login', { username, password })
            .subscribe(x => this.jwt = x.jwt);
    }

    public getJwt() {
        return this.jwt;
    }
}