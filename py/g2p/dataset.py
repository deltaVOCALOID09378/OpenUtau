# Made And Checked By DELTA SYNTH & Gemini AI
# Original by Patiphat Wongyai (Delta)
# ส่วนประกอบสำหรับการโหลดข้อมูล Grapheme และ Phoneme สำหรับ DiffSinger AI

import re
import torch
from torch.utils.data import Dataset
from typing import Dict, List, Tuple, Set, Any

# กำหนดดัชนีสำหรับสัญลักษณ์พิเศษที่เป็นมาตรฐาน
UNK_IDX, PAD_IDX, BOS_IDX, EOS_IDX = 0, 1, 2, 3
special_symbols = ['<unk>', '<pad>', '<bos>', '<eos>']

class SphinxDataset(Dataset):
    """
    คลาสสำหรับจัดการชุดข้อมูลพจนานุกรมเสียง (Phonetic Dictionary)
    รองรับการทำความสะอาดข้อมูลและการแปลงเป็น Tensor สำหรับการฝึกสอน AI
    """
    def __init__(self, path: str, cfg: Dict[str, Any],
                 comment_prefix: str = ';;;',
                 remove_word_digits: bool = True,
                 remove_phoneme_digits: bool = True):
        
        # ตั้งค่า Graphemes (ตัวอักษร) จาก Configuration
        self.graphemes: List[str] = cfg['encoder']['graphemes']
        self._validate_symbols(self.graphemes, "graphemes")
        self.grapheme_indexs: Dict[str, int] = {v: i for i, v in enumerate(self.graphemes)}

        # ตั้งค่า Phonemes (สัญลักษณ์เสียง) จาก Configuration
        self.phonemes: List[str] = cfg['decoder']['phonemes']
        self._validate_symbols(self.phonemes, "phonemes")
        self.phoneme_indexs: Dict[str, int] = {v: i for i, v in enumerate(self.phonemes)}

        # โหลดและประมวลผลไฟล์พจนานุกรม
        self.entries: List[Tuple[str, List[str]]] = self.load_dict(
            path, comment_prefix, remove_word_digits, remove_phoneme_digits)

    def _validate_symbols(self, symbols: List[str], name: str) -> None:
        """ตรวจสอบความถูกต้องของสัญลักษณ์พิเศษในลำดับแรกๆ"""
        assert len(symbols) > 4, f"จำนวน {name} น้อยเกินไป"
        for i, sym in enumerate(special_symbols):
            assert symbols[i] == sym, f"{name}[{i}] ต้องเป็น {sym}"

    def __len__(self) -> int:
        return len(self.entries)

    def __getitem__(self, idx) -> Tuple[torch.Tensor, torch.Tensor]:
        if torch.is_tensor(idx):
            idx = idx.tolist()
            
        src_raw, tgt_raw = self.entries[idx]
        
        # แปลงตัวอักษรและเสียงเป็นตัวเลข (Indices)
        # หากไม่พบสัญลักษณ์จะถูกแทนด้วย UNK_IDX (0)
        src = torch.tensor([self.grapheme_indexs.get(s, UNK_IDX)
                           for s in src_raw], dtype=torch.long)
        tgt = torch.tensor([self.phoneme_indexs.get(p, UNK_IDX)
                           for p in tgt_raw], dtype=torch.long)
        
        return src, tgt

    def load_dict(self, path: str, comment_prefix: str,
                  remove_word_digits: bool,
                  remove_phoneme_digits: bool) -> List[Tuple[str, List[str]]]:
        """
        โหลดพจนานุกรมจากไฟล์และทำการ Pre-process ข้อมูล
        พร้อมระบบป้องกันการแครชจากการเข้ารหัส (Encoding)
        """
        # Regex สำหรับกำจัดเลขลำดับหลังคำหรือเสียง เช่น WORD(1) หรือ AA1 -> AA
        rm_digit = re.compile(r'\(\d+\)$|\d+$')
        entries: List[Tuple[str, List[str]]] = []
        ignored_graphemes: Set[str] = set()
        ignored_phonemes: Set[str] = set()

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

        # แสดงสรุปผลการโหลดข้อมูล
        print(f"--- สรุปการโหลดข้อมูลพจนานุกรม ---")
        print(f"จำนวนข้อมูลทั้งหมด: {len(entries)} รายการ")
        if ignored_graphemes:
            print(f"อักขระที่ถูกละเว้น: {', '.join(sorted(ignored_graphemes))}")
        if ignored_phonemes:
            print(f"สัญลักษณ์เสียงที่ถูกละเว้น: {', '.join(sorted(ignored_phonemes))}")
            
        return entries
