
import { Injectable } from '@angular/core';

@Injectable()


export abstract class StorageService {
    abstract getItem(key: string): string
    abstract setItem(key: string, val: string): void
    abstract removeItem(key: string): void
    abstract checkKey(key: string): any
}



export class BrowserStorage extends StorageService {
    getItem(key: string) {
        return JSON.parse(sessionStorage.getItem(key) || '');
    }
    setItem(key: string, val: string) {
        sessionStorage.setItem(key, JSON.stringify(val));
        
    }
    removeItem(key: string) {
        sessionStorage.removeItem(key);
    }
    checkKey(key: string) {
        return sessionStorage[key]
    }
}

export class ServerStorage extends StorageService {
    getItem(key: string) {
        return '';
    }
    setItem(key: string, val: string) {

    }
    removeItem(key: string) {

    }
    checkKey(key: string) {
        return false
    }
}