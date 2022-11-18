import re

ops = {
    "add": lambda x, y: y - x,
    "sub": lambda x, y: x + y,
    "mul": lambda x, y: y / x,
    "xor": lambda x, y: x ^ y,
}

# read the file
with open("./Inter/inter.ll") as f:
    lines = f.read()
    lines = lines.split("\n")

# grep the instructions
next_pos = 0
instructions = []
for line in lines:
    if re.search(r"getelementptr", line):
        next_pos = int(re.search(r"\s\d+,", line).group().strip().strip(","))

    res = re.search(r"(sub|icmp|add|mul|xor).*", line)
    if res:
        op = res.group(1)
        text = res.group(0)
        num = int(re.search(r"\s\d+,", text).group().strip().strip(","))
        pos = next_pos
        next_pos = 0
        instructions.append((op, num, pos))

flag = [""] * 100
for i in range(0, len(instructions), 2):
    op, num, pos = instructions[i]
    op2, num2, _ = instructions[i + 1]
    res = int(ops[op](num, num2))
    flag[pos] = chr(res)

print("".join(flag))
