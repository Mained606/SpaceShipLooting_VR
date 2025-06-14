# SpaceShipLooting VR

우주선을 배경으로 한 VR 액션 게임으로, 플레이어가 우주선 내부를 탐험하며 보스와 전투를 벌이는 게임입니다.

## 프로젝트 개요

- **개발 기간**: 2023.11.25 - 2024.12.31 (약 5주)
- **개발 인원**: 5명
- **개발 환경**: 
  - Unity 6000.0.26f1 (URP)
  - Meta Quest
  - Visual Studio 2022

- **시연 영상**: [Youtube](https://youtu.be/NVzRJA02070)

## 주요 기술 스택

- **Unity XR**
  - Oculus Integration
  - XR Interaction Toolkit
- **렌더링**
  - Universal Render Pipeline (URP)
  - Custom Shader
  - VFX Graph
- **디자인 패턴**
  - 상태 패턴 (보스 AI)
  - 싱글톤 패턴 (매니저 클래스)

## 주요 기능

### 1. VR 플레이어 시스템
- 거리 기반 적 인식 심박 사운드 시스템
- 나이트 비전 시스템
- 무기 상호작용 시스템

### 2. 보스 AI
- 상태 머신 기반 AI 로직
- 다양한 공격 패턴
- 페이즈 시스템

### 3. 인터랙션 시스템
- VR 컨트롤러 기반 상호작용
- 커스텀 아웃라인 시스템
- 아이템 획득/사용 시스템

### 4. 게임 시스템
- 씬 관리 시스템
- 세이브/로드 시스템
- 데미지 처리 시스템

## 프로젝트 구조

```
Assets/
├── SpaceShipLooting/
│   ├── Script/
│   │   ├── Player/     # 플레이어 관련 스크립트
│   │   ├── Boss/       # 보스 AI 스크립트
│   │   ├── Weapons/    # 무기 시스템
│   │   ├── Managers/   # 게임 매니저
│   │   └── Interactable/ # 상호작용 시스템
│   ├── Scenes/         # 게임 씬
│   ├── Prefabs/        # 프리팹
│   └── Materials/      # 머티리얼 및 셰이더
```

## 주요 구현 내용

### VR 최적화
- 렌더링 파이프라인 최적화
- VR 컨트롤러 입력 처리
- 플레이어 장비 동기화

### 보스 AI 시스템
- 상태 머신 패턴 적용
- 다양한 공격 패턴 구현
- 페이즈별 특수 패턴

### 인터랙션 시스템
- XR Interaction Toolkit 활용
- 커스텀 아웃라인 셰이더
- 아이템 상호작용

## 성과
- 직관적인 VR 컨트롤러 조작성 구현
- 최적화된 그래픽 퍼포먼스
