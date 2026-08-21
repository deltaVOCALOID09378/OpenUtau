try:
            # ใช้ 'utf-8-sig' เพื่อข้าม BOM หากมี และ errors='replace' เพื่อป้องกันโปรแกรมหยุดทำงานเมื่อเจออักขระที่อ่านไม่ได้
            with open(path, 'r', encoding='utf-8-sig', errors='replace') as f:
                for line in f:
                    line = line.strip()
                    if not line or line.startswith(comment_prefix):
                        continue
                    
                    parts = line.split()
                    if len(parts) < 2:
                        continue
                    
                    word = parts[0]
                    pron = parts[1:]
                    
                    # ทำความสะอาดข้อมูลเลขต่อท้าย
                    if remove_word_digits:
                        word = rm_digit.sub('', word)
                    if remove_phoneme_digits:
                        pron = [rm_digit.sub('', p) for p in pron]
                        
                    # ตรวจสอบสัญลักษณ์ที่ไม่ได้อยู่ในชุดที่กำหนด (เพื่อเก็บ Log)
                    for c in word:
                        if c not in self.grapheme_indexs:
                            ignored_graphemes.add(c)
                    for p in pron:
                        if p not in self.phoneme_indexs:
                            ignored_phonemes.add(p)
                            
                    entries.append((word, pron))
                    
        except FileNotFoundError:
            print(f"[ข้อผิดพลาด] ไม่พบไฟล์พจนานุกรมที่: {path}")
        except UnicodeDecodeError as e:
            print(f"[ข้อผิดพลาด] ปัญหาการเข้ารหัสไฟล์ (Encoding) ที่ไฟล์ {path}: {e}")
        except Exception as e:
            print(f"[ข้อผิดพลาด] เกิดข้อผิดพลาดที่ไม่คาดคิดขณะอ่านไฟล์ {path}: {e}")
