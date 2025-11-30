import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class DbService {
  private dbName = 'SpaceXAppDB';
  private storeName = 'settings';
  private readonly dbPromise: Promise<IDBDatabase>;

  constructor() {
    this.dbPromise = new Promise((resolve, reject) => {
      const request = indexedDB.open(this.dbName, 1);
      request.onerror = () => reject('Error opening DB');
      request.onsuccess = () => resolve(request.result);
      request.onupgradeneeded = (event: any) => {
        event.target.result.createObjectStore(this.storeName);
      };
    });
  }

  async setSetting<T>(key: string, value: T): Promise<void> {
    const db = await this.dbPromise;
    return new Promise((resolve, reject) => {
      const tx = db.transaction(this.storeName, 'readwrite');
      tx.objectStore(this.storeName).put(value, key);
      tx.oncomplete = () => resolve();
      tx.onerror = () => reject();
    });
  }

  async getSetting<T>(key: string): Promise<T | null> {
    const db = await this.dbPromise;
    return new Promise((resolve) => {
      const tx = db.transaction(this.storeName, 'readonly');
      const request = tx.objectStore(this.storeName).get(key);
      request.onsuccess = () => resolve(request.result || null);
    });
  }

  async clearAllSettings(): Promise<void> {
    const db = await this.dbPromise;
    return new Promise((resolve, reject) => {
      const tx = db.transaction(this.storeName, 'readwrite');
      tx.objectStore(this.storeName).clear();
      tx.oncomplete = () => resolve();
      tx.onerror = () => reject("Error clearing DB");
    });
  }
}
