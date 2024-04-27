# homework-3

---

### Задание 

1. Написать консольное приложение, которое
* Принимает на вход текстовый файл со списком товаров (id), прогнозом продаж этого товара (prediction) и текущим количеством товара на складе (stock), 1 строка - 1 товар. **Файл большой, считать его в память полностью не получится.**
 * Расчитывает для каждого товара
потребность.

    
* Результат записывает в выходной файл. 
        
Пример входного файла (csv)
```
id, prediction, stock
123, 2, 1
456, 1, 3
```

Пример выходного файла
 ```
id, demand
123, 1
456, 0
 ```

2. Во время работы приложение должно отображать прогресс для каждого шага: количество прочитанных строк, количество расчитанных товаров, количество записанных результатов.
3. Должна быть возможность остановить (отменить) расчет.
4. Расчет должен происходить в многопоточном режиме.
4. Нужна возможность управлять степенью параллелизма расчета (через конфиг).

---

### Задание на 10 баллов
Реализовать управление параллелизмом "на лету", без перезапуска приложения и без потери прогресса.

---

💡 *Совет:* Так как операциия расчета довольно простая, в академических целях можно сымитировать ее сложность, снабдив дополнительными вычислениями, либо просто покрутив цикл.
